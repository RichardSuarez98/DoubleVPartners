export interface PersonasDTO {
    idPersona:            number;
    nombres:              string;
    apellidos:            string;
    numeroIdentificacion: string;
    email:                string;
    tipoIdentificacion:   string;
    fechaCreacion:        Date;
}

export interface PersonaCreacionDTO {
    nombres:              string;
    apellidos:            string;
    numeroIdentificacion: string;
    email:                string;
    tipoIdentificacion:   string;
}


export interface PersonaUsuarioCreacionDTO {
    nombres:              string;
    apellidos:            string;
    numeroIdentificacion: string;
    email:                string;
    tipoIdentificacion:   string;
    usuario:              string;
    pass:                 string;
}
