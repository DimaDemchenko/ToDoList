using System.ComponentModel.DataAnnotations;

namespace ToDoList.ValidationAttributes
{
    public class CategoryValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value != null && (int)value != -1)
            {
                return true;                
            }
            return false;
        }
    }
}
