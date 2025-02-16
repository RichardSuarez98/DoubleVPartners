CREATE PROCEDURE sp_a_persona
    @idPersona INT,
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @numeroIdentificacion VARCHAR(20),
    @email VARCHAR(100),
    @tipoIdentificacion VARCHAR(50),
    @mensaje VARCHAR(255) OUTPUT,
    @codigo INT OUTPUT
AS
BEGIN
    SET @mensaje = '';
    SET @codigo = 0;

    BEGIN TRY
        -- Update the tbl_Persona table with the new data
        UPDATE tbl_Persona
        SET nombres = @nombres,
            apellidos = @apellidos,
            numeroIdentificacion = @numeroIdentificacion,
            email = @email,
            tipoIdentificacion = @tipoIdentificacion
        WHERE idPersona = @idPersona;

        SET @mensaje = 'Persona y usuario actualizados correctamente.';
        SET @codigo = 0;
    END TRY
    BEGIN CATCH
        SET @mensaje = ERROR_MESSAGE();
        SET @codigo = 1;
    END CATCH;

    -- Return the status code
    RETURN @codigo;
END;

