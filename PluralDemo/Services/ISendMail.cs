namespace PluralDemo.Services {
    public interface ISendMail {
        void Send(string subject, string message);
    }
}