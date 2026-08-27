using HappyChat.Application.DTOs.Chat;
using HappyChat.Application.Interfaces;
using HappyChat.Desktop.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HappyChat.Desktop.ViewModels.Chat;

public sealed class ChatViewModel : ViewModelBase
{
    private readonly IChatService _chatService;

    private readonly CancellationTokenSource
        _cancellationTokenSource = new();

    private string _searchText = string.Empty;

    private string _messageText = string.Empty;

    private string _selectedConversationInitials =
        string.Empty;

    private string _selectedConversationAvatarBrush =
        "#2563EB";

    private string _selectedConversationStatus =
        string.Empty;

    private string _errorMessage =
        string.Empty;

    private ConversationItemViewModel?
        _selectedConversation;

    private OpenChatResponse?
        _openChat;

    private bool _isLoadingConversations;

    private bool _isLoadingChat;

    private bool _isLoadingMessages;

    private bool _isSendingMessage;

    public ChatViewModel(
        IChatService chatService)
    {
        _chatService = chatService;

        Conversations =
            new ObservableCollection<
                ConversationItemViewModel>();

        Messages =
            new ObservableCollection<
                MessageItemViewModel>();

        SelectConversationCommand =
            new AsyncRelayCommand<ConversationItemViewModel?>(
                SelectConversationAsync);

        NewChatCommand =
            new RelayCommand(
                NewChat);

        SendMessageCommand =
            new RelayCommand(
                SendMessage,
                () =>
                    CanSendMessage);

        SearchCommand =
            new RelayCommand(
                ApplySearch);

        _ = LoadAsync();
    }

    // =========================================================
    // Collections
    // =========================================================

    public ObservableCollection<
        ConversationItemViewModel>
        Conversations
    {
        get;
    }

    public ObservableCollection<
        MessageItemViewModel>
        Messages
    {
        get;
    }

    private readonly
        ObservableCollection<
            ConversationItemViewModel>
        _allConversations =
            new();

    // =========================================================
    // Search
    // =========================================================

    public string SearchText
    {
        get => _searchText;

        set
        {
            if (SetProperty(
                    ref _searchText,
                    value))
            {
                ApplySearch();
            }
        }
    }

    // =========================================================
    // Message
    // =========================================================

    public string MessageText
    {
        get => _messageText;

        set
        {
            if (SetProperty(
                    ref _messageText,
                    value))
            {
                OnPropertyChanged(
                    nameof(CanSendMessage));

                if (SendMessageCommand
                    is RelayCommand command)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }
    }

    public bool CanSendMessage =>
        !string.IsNullOrWhiteSpace(
            MessageText)
        &&
        !_isSendingMessage
        &&
        SelectedConversation is not null;

    // =========================================================
    // Selected Conversation
    // =========================================================

    public ConversationItemViewModel?
        SelectedConversation
    {
        get => _selectedConversation;

        private set
        {
            if (SetProperty(
                    ref _selectedConversation,
                    value))
            {
                OnPropertyChanged(
                    nameof(SelectedConversationName));

                OnPropertyChanged(
                    nameof(SelectedConversationInitials));

                OnPropertyChanged(
                    nameof(SelectedConversationAvatarBrush));

                OnPropertyChanged(
                    nameof(SelectedConversationStatus));

                OnPropertyChanged(
                    nameof(SelectedConversationIsOnline));

                OnPropertyChanged(
                    nameof(CanSendMessage));

                if (SendMessageCommand
                    is RelayCommand command)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }
    }

    public string SelectedConversationName =>
        _openChat?.Title
        ?? SelectedConversation?.Title
        ?? string.Empty;

    public string SelectedConversationInitials =>
        _selectedConversationInitials;

    public string SelectedConversationAvatarBrush =>
        _selectedConversationAvatarBrush;

    public string SelectedConversationStatus =>
        _selectedConversationStatus;

    public bool SelectedConversationIsOnline =>
        _openChat?.IsOnline
        ?? SelectedConversation?.IsOnline
        ?? false;

    // =========================================================
    // Loading
    // =========================================================

    public bool IsLoadingConversations
    {
        get => _isLoadingConversations;

        private set =>
            SetProperty(
                ref _isLoadingConversations,
                value);
    }

    public bool IsLoadingChat
    {
        get => _isLoadingChat;

        private set =>
            SetProperty(
                ref _isLoadingChat,
                value);
    }

    public bool IsLoadingMessages
    {
        get => _isLoadingMessages;

        private set =>
            SetProperty(
                ref _isLoadingMessages,
                value);
    }

    // =========================================================
    // Error
    // =========================================================

    public string ErrorMessage
    {
        get => _errorMessage;

        private set
        {
            if (SetProperty(
                    ref _errorMessage,
                    value))
            {
                OnPropertyChanged(
                    nameof(HasError));
            }
        }
    }

    public bool HasError =>
        !string.IsNullOrWhiteSpace(
            ErrorMessage);

    // =========================================================
    // Commands
    // =========================================================

    public ICommand
        SelectConversationCommand
    {
        get;
    }

    public ICommand
        NewChatCommand
    {
        get;
    }

    public ICommand
        SendMessageCommand
    {
        get;
    }

    public ICommand
        SearchCommand
    {
        get;
    }

    // =========================================================
    // Initial Load
    // =========================================================

    private async Task LoadAsync()
    {
        try
        {
            ErrorMessage = string.Empty;

            IsLoadingConversations = true;

            var chats =
                await _chatService.GetAllChatsAsync(
                    _cancellationTokenSource.Token);

            _allConversations.Clear();
            Conversations.Clear();

            for (var index = 0;
                 index < chats.Count;
                 index++)
            {
                var conversation =
                    new ConversationItemViewModel(
                        chats[index],
                        index);

                _allConversations.Add(
                    conversation);

                Conversations.Add(
                    conversation);
            }

            if (Conversations.Count > 0)
            {
                await SelectConversationAsync(
                    Conversations[0]);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
            ErrorMessage =
                "Unable to load your conversations.";
        }
        finally
        {
            IsLoadingConversations = false;
        }
    }

    // =========================================================
    // Open Chat
    // =========================================================

    private async Task SelectConversationAsync(
        ConversationItemViewModel? conversation)
    {
        if (conversation is null)
            return;

        if (ReferenceEquals(
                SelectedConversation,
                conversation)
            &&
            _openChat is not null)
        {
            return;
        }

        try
        {
            ErrorMessage = string.Empty;

            SetSelectedConversation(
                conversation);

            foreach (var item in Conversations)
            {
                item.IsSelected =
                    item == conversation;
            }

            _openChat = null;

            Messages.Clear();

            IsLoadingChat = true;

            var openChat =
                await _chatService.OpenChatAsync(
                    conversation.ChatId,
                    _cancellationTokenSource.Token);

            _openChat =
                openChat;

            UpdateHeader();

            OnPropertyChanged(
                nameof(SelectedConversationName));

            OnPropertyChanged(
                nameof(SelectedConversationIsOnline));

            if (openChat is null)
            {
                ErrorMessage =
                    "Unable to open this chat.";

                return;
            }

            await LoadMessagesAsync(
                conversation.ChatId);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
            ErrorMessage =
                "Unable to open this conversation.";
        }
        finally
        {
            IsLoadingChat = false;
        }
    }

    // =========================================================
    // Selection
    // =========================================================

    private void SetSelectedConversation(
        ConversationItemViewModel conversation)
    {
        if (ReferenceEquals(
                _selectedConversation,
                conversation))
        {
            conversation.IsSelected = true;
            return;
        }

        if (_selectedConversation is not null)
        {
            _selectedConversation.IsSelected = false;
        }

        conversation.IsSelected = true;

        SelectedConversation =
            conversation;
    }

    // =========================================================
    // Header
    // =========================================================

    private void UpdateHeader()
    {
        if (SelectedConversation is null)
            return;

        _selectedConversationInitials =
            SelectedConversation.Initials;

        _selectedConversationAvatarBrush =
            SelectedConversation.AvatarBrush;

        if (_openChat is not null)
        {
            _selectedConversationStatus =
                _openChat.IsOnline
                    ? "Active now"
                    : FormatLastSeen(
                        _openChat.LastSeen);
        }
        else
        {
            _selectedConversationStatus =
                SelectedConversation.IsOnline
                    ? "Active now"
                    : "Offline";
        }

        OnPropertyChanged(
            nameof(SelectedConversationInitials));

        OnPropertyChanged(
            nameof(SelectedConversationAvatarBrush));

        OnPropertyChanged(
            nameof(SelectedConversationStatus));

        OnPropertyChanged(
            nameof(SelectedConversationIsOnline));
    }

    private static string FormatLastSeen(
        DateTime? lastSeen)
    {
        if (lastSeen is null)
            return "Offline";

        var value =
            lastSeen.Value;

        if (value.Date == DateTime.Today)
            return $"Last seen {value:h:mm tt}";

        if (value.Date ==
            DateTime.Today.AddDays(-1))
        {
            return "Last seen yesterday";
        }

        return
            $"Last seen {value:MMM d}";
    }

    // =========================================================
    // Messages
    // =========================================================

    private async Task LoadMessagesAsync(
        int chatId)
    {
        try
        {
            IsLoadingMessages = true;

            var messages =
                await _chatService.GetMessagesAsync(
                    chatId,
                    page: 1,
                    pageSize: 30,
                    cancellationToken:
                        _cancellationTokenSource.Token);

            Messages.Clear();

            foreach (var message in messages)
            {
                Messages.Add(
                    new MessageItemViewModel(
                        message));
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
            ErrorMessage =
                "Unable to load messages.";
        }
        finally
        {
            IsLoadingMessages = false;
        }
    }

    // =========================================================
    // Search
    // =========================================================

    private void ApplySearch()
    {
        var query =
            SearchText.Trim();

        Conversations.Clear();

        if (string.IsNullOrWhiteSpace(query))
        {
            foreach (var conversation
                     in _allConversations)
            {
                Conversations.Add(
                    conversation);
            }

            return;
        }

        foreach (var conversation
                 in _allConversations)
        {
            if (conversation.Title.Contains(
                    query,
                    StringComparison.OrdinalIgnoreCase)
                ||
                conversation.LastMessage.Contains(
                    query,
                    StringComparison.OrdinalIgnoreCase))
            {
                Conversations.Add(
                    conversation);
            }
        }
    }

    // =========================================================
    // New Chat
    // =========================================================

    private void NewChat()
    {
        SearchText = string.Empty;

        if (Conversations.Count == 0)
            return;

        _ = SelectConversationAsync(
            Conversations[0]);
    }

    // =========================================================
    // Send Message
    // =========================================================

    private void SendMessage()
    {
        /*
         * فعلاً API ارسال پیام در اطلاعات Backend
         * که دادی وجود ندارد.
         *
         * بنابراین اینجا هنوز پیام را به Backend
         * ارسال نمی‌کنیم.
         *
         * بعداً با SendMessage API / SignalR / WebSocket
         * همین بخش را وصل می‌کنیم.
         */
    }

    // =========================================================
    // Dispose
    // =========================================================

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }
}