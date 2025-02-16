namespace DoubleVPartnersBackend.DTOs
{
    public class RouteResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public RouteResponse(Status status)
        {
            Code = GetCode(status);
            Message = GetMessage(status);
        }

        public RouteResponse(Status status, string message)
        {
            Code = GetCode(status);
            Message = message;
        }

        public static RouteResponse Response(Status status)
        {
            return new RouteResponse(status);
        }

        public enum Status
        {
            OK = 200,
            BAD_REQUEST = 400,
            BAD_REQUEST_CABECERA = 400,
            BAD_REQUEST_DETALLES = 400,
            INTERNAL_ERROR = 500,
            NOT_FOUND = 404,
            CONFLICT_WITH_DATA = 406,
            INCOMPLETE_DATA = 407
        }

        // Método para obtener el código del estado
        public static int GetCode(Status status)
        {
            return (int)status;
        }

        // Método para obtener el mensaje del estado
        public static string GetMessage(Status status)
        {
            switch (status)
            {
                case Status.OK:
                    return "La solicitud se ha procesado correctamente.";
                case Status.BAD_REQUEST:
                    return "Request incorrecto";
                case Status.INTERNAL_ERROR:
                    return "Hay un problema con la acción, revise la aplicación e inténtalo de nuevo.";
                case Status.NOT_FOUND:
                    return "Error al procesar la respuesta de Bloomberg";
                case Status.CONFLICT_WITH_DATA:
                    return "Existe un problema con los datos ingresados";
                case Status.INCOMPLETE_DATA:
                    return "La cantidad de detalles no coincide con las enviadas en cabecera";
                default:
                    return "Estado desconocido";
            }
        }
    }
}
