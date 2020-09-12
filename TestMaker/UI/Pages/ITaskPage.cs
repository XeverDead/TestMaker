using Lib;

namespace UI.Pages
{
    public interface ITaskPage
    {
        Task Task { get; }
        dynamic Answer { get; }
        bool IsAnswerChosen { get; }
        object Content { get; set; }
    }
}
