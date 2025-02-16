CREATE PROCEDURE sp_i_persona_usuario
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @numeroIdentificacion VARCHAR(20),
    @email VARCHAR(100),
    @tipoIdentificacion VARCHAR(50),
    @usuario VARCHAR(50),
    @pass VARCHAR(100),
    @mensaje VARCHAR(255) OUTPUT,
    @codigo INT OUTPUT
AS
BEGIN
    SET @mensaje = '';
    SET @codigo = 0;

    BEGIN TRY
        INSERT INTO tbl_Persona (nombres, apellidos, numeroIdentificacion, email, tipoIdentificacion, fechaCreacion)
        VALUES (@nombres, @apellidos, @numeroIdentificacion, @email, @tipoIdentificacion, GETDATE());

        -- Obtener el idPersona generado (en caso de usar un campo tipo IDENTITY)
        DECLARE @idPersona INT = SCOPE_IDENTITY();

        INSERT INTO tbl_Usuario (idPersona, usuario, pass, fechaCreacion)
        VALUES (@idPersona, @usuario, @pass, GETDATE());

        SET @mensaje = 'Persona y usuario registrados correctamente.';
        SET @codigo = 0;  
    END TRY
    BEGIN CATCH
        SET @mensaje = ERROR_MESSAGE();
        SET @codigo = 1;  
    END CATCH;

    -- Devolver el código de estado
    RETURN @codigo;
END;
