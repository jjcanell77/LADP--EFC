using LADP__EFC.Models;

namespace LADP__EFC.Repository
{
    public interface IRepositoryToDoItems
    {
        IEnumerable<TodoItemDTO> GetTodoItems();
        TodoItemDTO GetTodoItem(int id);
        TodoItemDTO PutTodoItem(TodoItemDTO todoDTO);
        TodoItemDTO PostTodoItem(TodoItemDTO todoDTO);
        TodoItemDTO DeleteTodoItem(int id);
    }
}
