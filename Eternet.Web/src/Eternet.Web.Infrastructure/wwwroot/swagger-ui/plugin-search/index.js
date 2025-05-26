/*
 * AdvancedFilterFullSearchPlugin
 *
 * Full‑text search plugin for Swagger‑UI. It performs case‑insensitive, token‑based
 * matching across paths, summaries, descriptions, operationId, path‑level
 * descriptions, parameters, request bodies, responses and any referenced
 * components. Tokens are split on whitespace and every token must be present.
 * Tags themselves are not part of the search scope. Intended for use with
 * SwaggerUIBundle({ filter: true }).
 */

(function (w) {
    const plugin = function (system) {

        //const log = (...a) => console.debug("[FullSearch]", ...a);

        const tokenize = p => (p || "").trim().toLowerCase().split(/\s+/).filter(Boolean);

        function buildComponentIndex(spec, tokens) {
            const map = new Map();
            if (!spec?.components) return map;
            tokens.forEach(t => map.set(t, new Set()));
            for (const [type, items] of Object.entries(spec.components)) {
                for (const [name, value] of Object.entries(items)) {
                    const ref = `#/components/${type}/${name}`;
                    const str = (name + JSON.stringify(value)).toLowerCase();
                    tokens.forEach(t => { if (str.includes(t)) map.get(t).add(ref); });
                }
            }
            return map;
        }

        function refsMatch(opStr, tokenRefs) {
            const refs = opStr.match(/"#\/components\/[^"]+"/g)?.map(r => r.replace(/"/g, "")) || [];
            return [...tokenRefs.values()].every(set => refs.some(r => set.has(r)));
        }

        return {
            fn: {
                tagFilter() {
                    return true;
                },

                opsFilter(taggedOps, phrase) {
                    const tokens = tokenize(window.fullSearchPhrase || phrase);
                    //log("tokens:", tokens);

                    if (tokens.length === 0) return system.Im.fromJS(taggedOps);

                    const spec = system.specSelectors.specJson().toJS();
                    const tokenRefs = buildComponentIndex(spec, tokens);
                    const result = JSON.parse(JSON.stringify(taggedOps));

                    for (const tag in result) {
                        const ops = result[tag].operations;
                        for (let i = ops.length - 1; i >= 0; i--) {
                            const wOp = ops[i];
                            const op = wOp.operation;
                            const haystack =
                                (wOp.path +
                                    (op.operationId || "") +
                                    (op.summary || "") +
                                    (op.description || "") +
                                    (spec.paths?.[wOp.path]?.description || "")
                                ).toLowerCase();

                            const opStr = JSON.stringify(op).toLowerCase();
                            const direct = tokens.every(t => haystack.includes(t) || opStr.includes(t));
                            const viaRef = refsMatch(opStr, tokenRefs);

                            //log(wOp.path, { direct, viaRef });

                            if (!direct && !viaRef) ops.splice(i, 1);
                        }
                        if (ops.length === 0) delete result[tag];
                        else result[tag].operations = ops;
                    }
                    return system.Im.fromJS(result);
                }
            }
        };
    };

    w.AdvancedFilterFullSearchPlugin = plugin;
})(window);
