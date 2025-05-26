export function getColumnWidths(gridElement) {
    const headers = gridElement.querySelectorAll('.column-header');
    return Array.from(headers).map(h => h.getBoundingClientRect().width + 'px');
}
