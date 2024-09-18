using System.Text.Json;
namespace LibraryBooks.Models.Shared
{
    public enum BootstrapColor
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info,
        Light,
        Dark,
        White,
        Black,
        Body,
        Muted,
        Transparent
    }

    public class ToastModel 
    {
        public string Message { get; set; } = string.Empty;
        public BootstrapColor BackgroundColor { get; set; } = BootstrapColor.Primary;
        public BootstrapColor TextColor { get; set; } = BootstrapColor.White;

        public ToastModel(string message, BootstrapColor backgroundColor = BootstrapColor.Primary, BootstrapColor textColor = BootstrapColor.White)
        {
            Message = message;
            BackgroundColor = backgroundColor;
            TextColor = textColor;
        }
    }
    public class ToastViewModel
    {
        
        public ToastViewModel() { }

        public ToastViewModel(List<ToastModel> toasts)
        {
            Toasts = toasts;
        }

        public ToastViewModel(string message, BootstrapColor backgroundColor = BootstrapColor.Primary, BootstrapColor textColor = BootstrapColor.White)
        {
            Toasts.Add(
                new ToastModel(message, backgroundColor, textColor));
        }

        public string AsJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public void AddToast(string message, BootstrapColor backgroundColor = BootstrapColor.Primary, BootstrapColor textColor = BootstrapColor.White)
        {
            Toasts.Add(new ToastModel(message, backgroundColor, textColor));
        }

        public List<ToastModel> Toasts { get; set; } = new List<ToastModel>();

    }
}
