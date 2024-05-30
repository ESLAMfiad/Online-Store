namespace Store.APIs.Extentions
{
    public static class AddSwagger
    {
        public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
