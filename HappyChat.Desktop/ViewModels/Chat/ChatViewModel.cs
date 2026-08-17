using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using HappyChat.Application.Interfaces;
using HappyChat.Desktop.Commands;
using HappyChat.Desktop.Helpers;
using HappyChat.Desktop.Services;
using HappyChat.Desktop.ViewModels.Auth;

namespace HappyChat.Desktop.ViewModels.Chat;

public sealed class ChatViewModel : ViewModelBase
{
    private readonly IChatService _chatService;
    private readonly INavigationService _navigationService;

    private ChatListItemViewModel? _selectedChat;
    private string _messageText = string.Empty;
    private bool _isLoadingChats;
    private bool _isLoadingMessages;

    public ChatViewModel(IChatService chatService, INavigationService navigationService)
    {
        _chatService = chatService;
        _navigationService = navigationService;

        Chats = new ObservableCollection<ChatListItemViewModel>();
        Messages = new ObservableCollection<MessageViewModel>();

        LoadChatsCommand = new AsyncRelayCommand(LoadChatsAsync);
        SendMessageCommand = new AsyncRelayCommand(SendMessageAsync);

        NavigateToSignUpDevCommand = new RelayCommand(() => _navigationService.NavigateTo<CreateAccountViewModel>());
        NavigateToLoginDevCommand = new RelayCommand(() => _navigationService.NavigateTo<LoginViewModel>());
        NavigateToOtpDevCommand = new RelayCommand(() => _navigationService.NavigateTo<VerifyOtpViewModel>());
    }

    public ObservableCollection<ChatListItemViewModel> Chats { get; }

    public ObservableCollection<MessageViewModel> Messages { get; }

    public ChatListItemViewModel? SelectedChat
    {
        get => _selectedChat;
        private set
        {
            if (SetProperty(ref _selectedChat, value))
            {
                OnPropertyChanged(nameof(HasSelectedChat));
                OnPropertyChanged(nameof(HasNoSelectedChat));
                OnPropertyChanged(nameof(SelectedChatName));
                OnPropertyChanged(nameof(SelectedChatInitials));
                OnPropertyChanged(nameof(SelectedChatAvatarBrush));
                OnPropertyChanged(nameof(SelectedChatIsOnline));
                OnPropertyChanged(nameof(SelectedChatStatusText));
            }
        }
    }

    public bool HasSelectedChat => SelectedChat is not null;

    public bool HasNoSelectedChat => !HasSelectedChat;

    public string SelectedChatName => SelectedChat?.Name ?? string.Empty;

    public string SelectedChatInitials => SelectedChat?.Initials ?? string.Empty;

    public IBrush SelectedChatAvatarBrush => SelectedChat?.AvatarBrush ?? AvatarColorPalette.GetColor(0);

    public bool SelectedChatIsOnline => SelectedChat?.IsOnline ?? false;

    public string SelectedChatStatusText => SelectedChatIsOnline ? "Active now" : "Offline";

    public string MessageText
    {
        get => _messageText;
        set => SetProperty(ref _messageText, value);
    }

    public bool IsLoadingChats
    {
        get => _isLoadingChats;
        private set => SetProperty(ref _isLoadingChats, value);
    }

    public bool IsLoadingMessages
    {
        get => _isLoadingMessages;
        private set => SetProperty(ref _isLoadingMessages, value);
    }

    public ICommand LoadChatsCommand { get; }

    public ICommand SendMessageCommand { get; }

    public ICommand NavigateToSignUpDevCommand { get; }

    public ICommand NavigateToLoginDevCommand { get; }

    public ICommand NavigateToOtpDevCommand { get; }

    private async Task LoadChatsAsync()
    {
        try
        {
            IsLoadingChats = true;

            var chats = await _chatService.GetChatsAsync();

            Chats.Clear();

            foreach (var dto in chats)
            {
                var item = new ChatListItemViewModel(
                    dto,
                    new RelayCommand(() => _ = SelectChatAsync(dto.Id)));

                Chats.Add(item);
            }
        }
        catch (Exception)
        {
            // TODO: نمایش خطای بارگذاری
        }
        finally
        {
            IsLoadingChats = false;
        }
    }

    private async Task SelectChatAsync(int chatId)
    {
        var chat = Chats.FirstOrDefault(x => x.Id == chatId);

        if (chat is null)
            return;

        foreach (var item in Chats)
        {
            item.IsSelected = item.Id == chatId;
        }

        SelectedChat = chat;

        await LoadMessagesAsync(chatId);
    }

    private async Task LoadMessagesAsync(int chatId)
    {
        try
        {
            IsLoadingMessages = true;

            Messages.Clear();

            var messages = await _chatService.GetMessagesAsync(chatId);

            foreach (var dto in messages)
            {
                Messages.Add(new MessageViewModel(dto, SelectedChatInitials));
            }
        }
        catch (Exception)
        {
            // TODO: نمایش خطای بارگذاری
        }
        finally
        {
            IsLoadingMessages = false;
        }
    }

    private async Task SendMessageAsync()
    {
        if (SelectedChat is null || string.IsNullOrWhiteSpace(MessageText))
            return;

        var text = MessageText;
        MessageText = string.Empty;

        try
        {
            var sent = await _chatService.SendMessageAsync(SelectedChat.Id, text);

            if (sent is not null)
            {
                Messages.Add(new MessageViewModel(sent, SelectedChatInitials));
            }
        }
        catch (Exception)
        {
            // TODO: نمایش خطای ارسال
        }
    }
}