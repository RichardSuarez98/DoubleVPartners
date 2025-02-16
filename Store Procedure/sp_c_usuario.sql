CREATE PROCEDURE sp_c_usuario
    @idUsuario INT = NULL,       -- Parámetro de entrada para el idPersona (puede ser NULL)
    @mensaje NVARCHAR(255) OUTPUT,  -- Variable de salida para el mensaje
    @codigo INT OUTPUT              -- Variable de salida para el código de estado (0 = éxito, 1 = error)
AS
BEGIN
    -- Inicializar las variables de salida
    SET @mensaje = '';  -- Iniciar el mensaje como vacío
    SET @codigo = 0;     -- Iniciar el código como éxito (0)

    -- Si se pasa un idPersona específico
    IF @idUsuario IS NOT NULL
    BEGIN
        -- Consultar por idPersona
        IF EXISTS (SELECT 1 FROM tbl_Usuario WHERE idUsuario = @idUsuario)
        BEGIN
            SELECT idUsuario, usuario
            FROM tbl_Usuario
            WHERE idUsuario = @idUsuario;

            -- Establecer el resultado a 1 si la consulta fue exitosa
            SET @mensaje = 'Consulta ejecutada correctamente.';
            SET @codigo = 0;  -- 0 significa éxito
        END
        ELSE
        BEGIN
            -- Si no se encuentra el idPersona
            SET @mensaje = 'No se encontró la persona con el id proporcionado.';
            SET @codigo = 1;  -- 1 significa que no se encontraron resultados
        END
    END
    ELSE
    BEGIN
        IF EXISTS (SELECT 1 FROM tbl_Usuario)
        BEGIN
            SELECT idUsuario, usuario
            FROM tbl_Usuario;

            -- Establecer el resultado a 1 si la consulta fue exitosa
            SET @mensaje = 'Consulta ejecutada correctamente.';
            SET @codigo = 0;  -- 0 significa éxito
        END
        ELSE
        BEGIN
            -- Si no existen resultados en la tabla
            SET @mensaje = 'No hay registros disponibles en tbl_Persona.';
            SET @codigo = 1;  -- 1 significa que no se encontraron resultados
        END
    END

    -- Devolver el código de estado
    RETURN @codigo;
END;



