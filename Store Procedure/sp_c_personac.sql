CREATE PROCEDURE sp_c_personac
    @mensaje NVARCHAR(255) OUTPUT,  -- Variable de salida para el mensaje
    @codigo INT OUTPUT              -- Variable de salida para el código de estado (0 = éxito, 1 = error)
AS
BEGIN
    -- Inicializar las variables de salida
    SET @mensaje = '';  -- Iniciar el mensaje como vacío
    SET @codigo = 0;     -- Iniciar el código como éxito (0)

    -- Verificar si existen filas en la tabla tbl_Persona
    IF EXISTS (SELECT 1 FROM tbl_Persona)
    BEGIN
        -- Ejecutar la consulta si existen resultados
        SELECT idPersona, nombres, apellidos, numeroIdentificacion, email, tipoIdentificacion, fechaCreacion
        FROM tbl_Persona;

        -- Establecer el resultado a 1 si la consulta fue exitosa
        SET @mensaje = 'Consulta ejecutada correctamente.';
        SET @codigo = 0;  -- 0 significa éxito
    END
    ELSE
    BEGIN
        -- Si no existen resultados, establecer el mensaje adecuado
        SET @mensaje = 'No hay registros disponibles en tbl_Persona.';
        SET @codigo = 1;  -- 1 significa que no se encontraron resultados
    END

    -- Devolver el código de estado
    RETURN @codigo;
END;
