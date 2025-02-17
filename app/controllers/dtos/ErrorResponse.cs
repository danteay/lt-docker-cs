namespace app.controllers.dtos
{
    public struct ErrorResponse(string message)
    {
        public string Message { get; set; } = message;
    }
}
