using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Components;

namespace Eternet.Web.UI.Tests.Components.DataGrid;

internal class DummyToastService : IToastService
{
    public event Action<bool>? OnClearAll;
    public event Action<ToastIntent, bool>? OnClearIntent;
    public event Action? OnClearQueue;
    public event Action<ToastIntent>? OnClearQueueIntent;
    public event Action<Type?, ToastParameters, object>? OnShow;
    public event Action<string?, ToastParameters>? OnUpdate;
    public event Action<string>? OnClose;

    public void ClearAll(bool includeQueue = true)
    {
        throw new NotImplementedException();
    }

    public void ClearCustomIntentToasts(bool includeQueue = true)
    {
        throw new NotImplementedException();
    }

    public void ClearDownloadToasts(bool includeQueue = true)
    {
        throw new NotImplementedException();
    }

    public void ClearErrorToasts(bool includeQueue = true)
    {
        throw new NotImplementedException();
    }

    public void ClearEventToasts(bool includeQueue = true)
    {
        throw new NotImplementedException();
    }

    public void ClearInfoToasts(bool includeQueue = true)
    {
        throw new NotImplementedException();
    }

    public void ClearIntent(ToastIntent intent, bool includeQueue = true)
    {
        throw new NotImplementedException();
    }

    public void ClearMentionToasts(bool includeQueue = true)
    {
        throw new NotImplementedException();
    }

    public void ClearProgressToasts(bool includeQueue = true)
    {
        throw new NotImplementedException();
    }

    public void ClearQueue()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueCustomIntentToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueDownloadToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueErrorToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueEventToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueInfoToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueMentionToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueProgressToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueSuccessToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueToasts(ToastIntent intent)
    {
        throw new NotImplementedException();
    }

    public void ClearQueueUploadToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueWarningToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearSuccessToasts(bool includeQueue = true)
    {
        throw new NotImplementedException();
    }

    public void ClearUploadToasts(bool includeQueue = true)
    {
        throw new NotImplementedException();
    }

    public void ClearWarningToasts(bool includeQueue = true)
    {
        throw new NotImplementedException();
    }

    public void CloseToast(string id)
    {
        throw new NotImplementedException();
    }

    public void ShowCommunicationToast(ToastParameters<CommunicationToastContent> parameters)
    {
        throw new NotImplementedException();
    }

    public void ShowConfirmationToast(ToastParameters<ConfirmationToastContent> parameters)
    {
        throw new NotImplementedException();
    }

    public void ShowCustom(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null, (Icon Value, Color Color)? icon = null)
    {
        throw new NotImplementedException();
    }

    public void ShowDownload(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
    {
        throw new NotImplementedException();
    }


    public void ShowError(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
    {
        throw new NotImplementedException();
    }


    public void ShowEvent(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
    {
        throw new NotImplementedException();
    }

    public void ShowInfo(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
    {
        throw new NotImplementedException();
    }

    public void ShowMention(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
    {
        throw new NotImplementedException();
    }


    public void ShowProgress(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
    {
        throw new NotImplementedException();
    }

    public void ShowProgressToast(ToastParameters<ProgressToastContent> parameters)
    {
        throw new NotImplementedException();
    }

    public void ShowSuccess(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
    {
        throw new NotImplementedException();
    }


    public void ShowToast(ToastIntent intent, string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
    {
        throw new NotImplementedException();
    }

    public void ShowToast<TContent>(Type? toastComponent, ToastParameters parameters, TContent content) where TContent : class
    {
        throw new NotImplementedException();
    }

    public void ShowToast<T, TContent>(ToastParameters<TContent> parameters)
        where T : IToastContentComponent<TContent>
        where TContent : class
    {
        throw new NotImplementedException();
    }

    public void ShowUpload(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
    {
        throw new NotImplementedException();
    }


    public void ShowWarning(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
    {
        throw new NotImplementedException();
    }

    public void UpdateToast<TContent>(string id, ToastParameters<TContent> parameters) where TContent : class
    {
        throw new NotImplementedException();
    }
}
