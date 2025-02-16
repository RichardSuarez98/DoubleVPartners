CREATE PROCEDURE sp_e_persona_usuario
    @idPersona INT,
    @mensaje VARCHAR(255) OUTPUT,
    @codigo INT OUTPUT
AS
BEGIN
    SET @mensaje = '';
    SET @codigo = 0;

    BEGIN TRY

	-- Optionally, you may want to delete associated user data from another table if necessary (e.g., tbl_Usuario)
        DELETE FROM tbl_Usuario
        WHERE idPersona = @idPersona;

        -- Delete from the tbl_Persona table
        DELETE FROM tbl_Persona
        WHERE idPersona = @idPersona;

        SET @mensaje = 'Persona y usuario eliminados correctamente.';
        SET @codigo = 0;
    END TRY
    BEGIN CATCH
        SET @mensaje = ERROR_MESSAGE();
        SET @codigo = 1;
    END CATCH;

    -- Return the status code
    RETURN @codigo;
END;
