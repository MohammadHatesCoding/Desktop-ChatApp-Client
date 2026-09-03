using HappyChat.Application.DTOs.Chat;
using HappyChat.Application.Interfaces;
using HappyChat.Desktop.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HappyChat.Desktop.ViewModels.Chat;

public sealed class ChatViewModel : ViewModelBase
{
    private readonly IChatService _chatService;
    private readonly IMessageService _messageService;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly ObservableCollection<ConversationItemViewModel> _allConversations = new();

    private string _searchText = string.Empty;
    private string _messageText = string.Empty;

    private string _selectedConversationInitials = string.Empty;
    private string _selectedConversationAvatarBrush = "#2563EB";
    private string _selectedConversationStatus = string.Empty;
    private MessageItemViewModel? _replyingToMessage;

    private string _errorMessage = string.Empty;

    private ConversationItemViewModel? _selectedConversation;
    private OpenChatResponse? _openChat;

    private bool _isLoadingConversations;
    private bool _isLoadingChat;
    private bool _isLoadingMessages;
    private bool _isSendingMessage;

    private const int MessagePageSize = 30;
    private int _currentMessagePage = 1;
    private bool _hasMoreMessages = true;
    private bool _isLoadingOlderMessages;

    public ChatViewModel(IChatService chatService, IMessageService messageService)
    {
        _chatService = chatService;
        _messageService = messageService;
        Conversations = new ObservableCollection<ConversationItemViewModel>();
        Messages = new ObservableCollection<MessageItemViewModel>();

        SelectConversationCommand = new AsyncRelayCommand<ConversationItemViewModel?>(SelectConversationAsync);
        NewChatCommand = new RelayCommand(NewChat);
        SendMessageCommand = new RelayCommand(SendMessage, () => CanSendMessage);
        SearchCommand = new RelayCommand(ApplySearch);
        CancelReplyCommand = new RelayCommand(CancelReply);
    }


    public ObservableCollection<ConversationItemViewModel> Conversations { get; }

    public ObservableCollection<MessageItemViewModel> Messages { get; }


    public ICommand SelectConversationCommand { get; }

    public ICommand NewChatCommand { get; }

    public ICommand SendMessageCommand { get; }

    public ICommand SearchCommand { get; }

    public ICommand CancelReplyCommand { get; }

    public event Action<int>? ScrollToMessageRequested;

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                ApplySearch();
            }
        }
    }

    public bool IsLoadingOlderMessages
    {
        get => _isLoadingOlderMessages;
        private set => SetProperty(ref _isLoadingOlderMessages, value);
    }

    public bool HasMoreMessages => _hasMoreMessages;

    public void ScrollToMessage(int messageId)
    {
        ScrollToMessageRequested?.Invoke(messageId);
    }

    public MessageItemViewModel? ReplyingToMessage
    {
        get => _replyingToMessage;
        private set
        {
            if (SetProperty(ref _replyingToMessage, value))
            {
                OnPropertyChanged(nameof(IsReplying));
                OnPropertyChanged(nameof(ReplyPreviewText));
            }
        }
    }


    public bool IsReplying =>
        ReplyingToMessage is not null;


    public string ReplyPreviewText => ReplyingToMessage is null ? string.Empty : ReplyingToMessage.Text.Length > 50
                ? ReplyingToMessage.Text[..50] + "..." : ReplyingToMessage.Text;


    public string MessageText
    {
        get => _messageText;
        set
        {
            if (SetProperty(ref _messageText, value))
            {
                OnPropertyChanged(nameof(CanSendMessage));

                if (SendMessageCommand is RelayCommand command)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }
    }


    public bool CanSendMessage =>
        !string.IsNullOrWhiteSpace(MessageText) &&
        !_isSendingMessage &&
        SelectedConversation is not null;


    public ConversationItemViewModel? SelectedConversation
    {
        get => _selectedConversation;
        private set
        {
            if (SetProperty(ref _selectedConversation, value))
            {
                OnPropertyChanged(nameof(SelectedConversationName));
                OnPropertyChanged(nameof(SelectedConversationInitials));
                OnPropertyChanged(nameof(SelectedConversationAvatarBrush));
                OnPropertyChanged(nameof(SelectedConversationStatus));
                OnPropertyChanged(nameof(SelectedConversationIsOnline));
                OnPropertyChanged(nameof(HasSelectedConversation));
                OnPropertyChanged(nameof(CanSendMessage));

                if (SendMessageCommand is RelayCommand command)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }
    }


    public bool HasSelectedConversation => SelectedConversation is not null;


    public string SelectedConversationName =>
        _openChat?.Title ??
        SelectedConversation?.Title ??
        "Select a conversation";


    public string SelectedConversationInitials =>
        HasSelectedConversation
            ? _selectedConversationInitials
            : string.Empty;


    public string SelectedConversationAvatarBrush =>
        _selectedConversationAvatarBrush;


    public string SelectedConversationStatus =>
        _selectedConversationStatus;


    public bool SelectedConversationIsOnline =>
        _openChat?.IsOnline ??
        SelectedConversation?.IsOnline ??
        false;


    public bool IsLoadingConversations
    {
        get => _isLoadingConversations;
        private set => SetProperty(ref _isLoadingConversations, value);
    }


    public bool IsLoadingChat
    {
        get => _isLoadingChat;
        private set => SetProperty(ref _isLoadingChat, value);
    }


    public bool IsLoadingMessages
    {
        get => _isLoadingMessages;
        private set => SetProperty(ref _isLoadingMessages, value);
    }


    public string ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            if (SetProperty(ref _errorMessage, value))
            {
                OnPropertyChanged(nameof(HasError));
            }
        }
    }


    public bool HasError =>
        !string.IsNullOrWhiteSpace(ErrorMessage);


    public async Task InitializeAsync()
    {
        await LoadAsync();
    }


    private void SetSelectedConversation(ConversationItemViewModel conversation)
    {
        if (ReferenceEquals(_selectedConversation, conversation))
        {
            conversation.IsSelected = true;
            return;
        }

        if (_selectedConversation is not null)
        {
            _selectedConversation.IsSelected = false;
        }

        conversation.IsSelected = true;

        SelectedConversation = conversation;
    }


    private void UpdateHeader()
    {
        if (SelectedConversation is null)
            return;

        _selectedConversationInitials = SelectedConversation.Initials;
        _selectedConversationAvatarBrush = SelectedConversation.AvatarBrush;

        if (_openChat is not null)
        {
            _selectedConversationStatus =
                _openChat.IsOnline
                    ? "Active now"
                    : FormatLastSeen(_openChat.LastSeen);
        }
        else
        {
            _selectedConversationStatus =
                SelectedConversation.IsOnline
                    ? "Active now"
                    : "Offline";
        }

        OnPropertyChanged(nameof(SelectedConversationInitials));
        OnPropertyChanged(nameof(SelectedConversationAvatarBrush));
        OnPropertyChanged(nameof(SelectedConversationStatus));
        OnPropertyChanged(nameof(SelectedConversationIsOnline));
    }


    private static string FormatLastSeen(DateTime? lastSeen)
    {
        if (lastSeen is null)
            return "Offline";

        var value = lastSeen.Value;

        if (value.Date == DateTime.Today)
            return $"Last seen {value:h:mm tt}";

        if (value.Date == DateTime.Today.AddDays(-1))
            return "Last seen yesterday";

        return $"Last seen {value:MMM d}";
    }


    private async Task LoadAsync()
    {
        try
        {
            ErrorMessage = string.Empty;
            IsLoadingConversations = true;

            var chats = await _chatService.GetAllChatsAsync(_cancellationTokenSource.Token);

            _allConversations.Clear();
            Conversations.Clear();

            for (var index = 0; index < chats.Count; index++)
            {
                var conversation = new ConversationItemViewModel(chats[index], index);

                _allConversations.Add(conversation);
                Conversations.Add(conversation);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load your conversations.";
        }
        finally
        {
            IsLoadingConversations = false;
        }
    }


    private async Task SelectConversationAsync(ConversationItemViewModel? conversation)
    {
        if (conversation is null)
            return;

        if (ReferenceEquals(SelectedConversation, conversation) &&
            _openChat is not null)
        {
            return;
        }

        try
        {
            ErrorMessage = string.Empty;

            SetSelectedConversation(conversation);

            foreach (var item in Conversations)
            {
                item.IsSelected = item == conversation;
            }

            _openChat = null;

            OnPropertyChanged(nameof(SelectedConversationName));
            OnPropertyChanged(nameof(SelectedConversationStatus));
            OnPropertyChanged(nameof(SelectedConversationIsOnline));

            Messages.Clear();

            IsLoadingChat = true;

            var openChat = await _chatService.OpenChatAsync(
                conversation.ChatId,
                _cancellationTokenSource.Token);

            if (openChat is null)
            {
                ErrorMessage = "Unable to open this chat.";
                return;
            }

            _openChat = openChat;

            UpdateHeader();

            OnPropertyChanged(nameof(SelectedConversationName));
            OnPropertyChanged(nameof(SelectedConversationStatus));
            OnPropertyChanged(nameof(SelectedConversationIsOnline));

            await LoadMessagesAsync(conversation.ChatId);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to open this conversation.";
        }
        finally
        {
            IsLoadingChat = false;
        }
    }


    private async Task LoadMessagesAsync(int chatId)
    {
        try
        {
            IsLoadingMessages = true;

            _currentMessagePage = 1;
            _hasMoreMessages = true;

            OnPropertyChanged(nameof(HasMoreMessages));

            var messages = await _chatService.GetMessagesAsync(
                chatId,
                page: 1,
                pageSize: MessagePageSize,
                cancellationToken: _cancellationTokenSource.Token);

            Messages.Clear();

            foreach (var message in messages
                         .OrderBy(x => x.SentAt)
                         .ThenBy(x => x.Id))
            {
                Messages.Add(new MessageItemViewModel(message));
            }

            if (messages.Count < MessagePageSize)
            {
                _hasMoreMessages = false;
                OnPropertyChanged(nameof(HasMoreMessages));
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load messages.";
        }
        finally
        {
            IsLoadingMessages = false;
        }
    }

    public async Task LoadOlderMessagesAsync()
    {
        if (SelectedConversation is null)
            return;

        if (IsLoadingOlderMessages)
            return;

        if (!_hasMoreMessages)
            return;

        try
        {
            IsLoadingOlderMessages = true;

            var nextPage = _currentMessagePage + 1;

            var messages = await _chatService.GetMessagesAsync(
                SelectedConversation.ChatId,
                page: nextPage,
                pageSize: MessagePageSize,
                cancellationToken: _cancellationTokenSource.Token);

            if (messages.Count == 0)
            {
                _hasMoreMessages = false;
                OnPropertyChanged(nameof(HasMoreMessages));
                return;
            }

            var olderMessages = messages
                .OrderBy(x => x.SentAt)
                .ThenBy(x => x.Id)
                .ToList();

            var existingIds = Messages
                .Select(x => x.Id)
                .ToHashSet();

            var newMessages = olderMessages
                .Where(x => !existingIds.Contains(x.Id))
                .Select(x => new MessageItemViewModel(x))
                .ToList();

            for (var i = newMessages.Count - 1; i >= 0; i--)
            {
                Messages.Insert(0, newMessages[i]);
            }

            _currentMessagePage = nextPage;

            if (messages.Count < MessagePageSize)
            {
                _hasMoreMessages = false;
                OnPropertyChanged(nameof(HasMoreMessages));
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load older messages.";
        }
        finally
        {
            IsLoadingOlderMessages = false;
        }
    }

    private void ApplySearch()
    {
        var query = SearchText.Trim();

        Conversations.Clear();

        if (string.IsNullOrWhiteSpace(query))
        {
            foreach (var conversation in _allConversations)
            {
                Conversations.Add(conversation);
            }

            return;
        }

        foreach (var conversation in _allConversations)
        {
            if (conversation.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                conversation.LastMessage.Contains(query, StringComparison.OrdinalIgnoreCase))
            {
                Conversations.Add(conversation);
            }
        }
    }


    private void NewChat()
    {
        SearchText = string.Empty;

        // فعلاً API ساخت چت وجود ندارد.
        // بعداً با CreateChat API جایگزین می‌شود.
    }


    public void ReplyToMessage(MessageItemViewModel message)
    {
        ReplyingToMessage = message;
    }

    public void CancelReply()
    {
        ReplyingToMessage = null;
    }

    // =========================================================
    // Send Message
    // =========================================================

    private async void SendMessage()
    {
        if (!CanSendMessage)
            return;

        if (SelectedConversation is null)
            return;

        var content = MessageText.Trim();

        if (string.IsNullOrWhiteSpace(content))
            return;

        try
        {
            ErrorMessage = string.Empty;

            _isSendingMessage = true;

            await _messageService.SendMessage(ChatId: SelectedConversation.ChatId, ReceiverUserId: null, 
                Content: content, RepliedTo: ReplyingToMessage?.Id, cancellationToken: _cancellationTokenSource.Token);

            MessageText = string.Empty;

            ReplyingToMessage = null;

            await RefreshLatestMessagesAsync(SelectedConversation.ChatId);
        }
        catch (OperationCanceledException)
        {

        }
        catch (Exception)
        {
            ErrorMessage = "Unable to send message.";
        }
        finally
        {
            _isSendingMessage = false;
        }
    }

    private async Task RefreshLatestMessagesAsync(int chatId)
    {
        var messages = await _chatService.GetMessagesAsync(
            chatId,
            page: 1,
            pageSize: MessagePageSize,
            cancellationToken: _cancellationTokenSource.Token);

        var existingIds = Messages
            .Select(x => x.Id)
            .ToHashSet();

        foreach (var message in messages
                     .OrderBy(x => x.SentAt)
                     .ThenBy(x => x.Id))
        {
            if (existingIds.Contains(message.Id))
                continue;

            Messages.Add(new MessageItemViewModel(message));
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }
}