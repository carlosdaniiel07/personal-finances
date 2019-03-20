namespace PersonalFinances.Services.Exceptions
{
    public class InvoiceNotFoundException : NotFoundException
    {
        public InvoiceNotFoundException (string message)
            : base(message)
        {

        }
    }
}