namespace UI.Pages
{
    public interface ITaskPage
    {
        dynamic Answer { get; }
        bool IsAnswerChosen { get; }
        object Content { get; set; }
    }
}
