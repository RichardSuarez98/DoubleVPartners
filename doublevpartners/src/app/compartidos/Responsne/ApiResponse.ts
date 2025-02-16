export interface ApiResponse {
    response: RouteResponse;
    data: any;
}

export interface RouteResponse {
    statusCode: number;
    message: string;
}
