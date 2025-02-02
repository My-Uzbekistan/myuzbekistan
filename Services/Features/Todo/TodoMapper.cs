using Riok.Mapperly.Abstractions;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

[Mapper]
public static partial class TodoMapper 
{
    #region Usable
    public static TodoView MapToView(this TodoEntity src) => src.To();
    public static List<TodoView> MapToViewList(this List<TodoEntity> src)=> src.ToList();
    public static TodoEntity MapFromView(this TodoView src) => src.From();
    #endregion

    #region Internal

    private static partial TodoView To(this TodoEntity src);
    private static partial List<TodoView> ToList(this List<TodoEntity> src);
    private static partial TodoEntity From(this TodoView TodoView);
    public static partial void From(TodoView personView, TodoEntity personEntity);

    #endregion
}