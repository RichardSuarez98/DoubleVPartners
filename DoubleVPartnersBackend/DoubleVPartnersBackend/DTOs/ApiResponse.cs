namespace DoubleVPartnersBackend.DTOs
{
    public class ApiResponse
    {
        public RouteResponse Response { get; set; }
        public object Data { get; set; }

        public ApiResponse(RouteResponse response, object data = null)
        {
            Response = response;
            Data = data;
        }
    }
}
