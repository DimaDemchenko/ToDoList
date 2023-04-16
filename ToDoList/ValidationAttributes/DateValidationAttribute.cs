using System.ComponentModel.DataAnnotations;

namespace ToDoList.ValidationAttributes
{
    public class DateValidationAttribute :ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value != null)
            {
                var date = (DateTime)value; 
                if(date < DateTime.Now)
                    return false;
                return true;
            }

            return false;
        }
    }
}
