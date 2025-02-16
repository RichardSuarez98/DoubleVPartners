export interface CredencialesUsuarioDTO{
    usuario: string;
    password: string;
}

export interface RespuestaAutenticacionDTO{
    token: string;
    expiracion: Date;
}