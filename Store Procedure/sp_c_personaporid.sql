USE [DoubleVPartners]
GO
/****** Object:  StoredProcedure [dbo].[sp_c_personac]    Script Date: 14/2/2025 19:08:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_c_personac]
    @idPersona INT = NULL,       -- Par�metro de entrada para el idPersona (puede ser NULL)
    @mensaje NVARCHAR(255) OUTPUT,  -- Variable de salida para el mensaje
    @codigo INT OUTPUT              -- Variable de salida para el c�digo de estado (0 = �xito, 1 = error)
AS
BEGIN
    -- Inicializar las variables de salida
    SET @mensaje = '';  -- Iniciar el mensaje como vac�o
    SET @codigo = 0;     -- Iniciar el c�digo como �xito (0)

    -- Si se pasa un idPersona espec�fico
    IF @idPersona IS NOT NULL
    BEGIN
        -- Consultar por idPersona
        IF EXISTS (SELECT 1 FROM tbl_Persona WHERE idPersona = @idPersona)
        BEGIN
            SELECT idPersona, nombres, apellidos, numeroIdentificacion, email, tipoIdentificacion, fechaCreacion
            FROM tbl_Persona
            WHERE idPersona = @idPersona;

            -- Establecer el resultado a 1 si la consulta fue exitosa
            SET @mensaje = 'Consulta ejecutada correctamente.';
            SET @codigo = 0;  -- 0 significa �xito
        END
        ELSE
        BEGIN
            -- Si no se encuentra el idPersona
            SET @mensaje = 'No se encontr� la persona con el id proporcionado.';
            SET @codigo = 1;  -- 1 significa que no se encontraron resultados
        END
    END
    ELSE
    BEGIN
        -- Si no se pasa un idPersona, consultar todos los registros
        IF EXISTS (SELECT 1 FROM tbl_Persona)
        BEGIN
            SELECT idPersona, nombres, apellidos, numeroIdentificacion, email, tipoIdentificacion, fechaCreacion
            FROM tbl_Persona;

            -- Establecer el resultado a 1 si la consulta fue exitosa
            SET @mensaje = 'Consulta ejecutada correctamente.';
            SET @codigo = 0;  -- 0 significa �xito
        END
        ELSE
        BEGIN
            -- Si no existen resultados en la tabla
            SET @mensaje = 'No hay registros disponibles en tbl_Persona.';
            SET @codigo = 1;  -- 1 significa que no se encontraron resultados
        END
    END

    -- Devolver el c�digo de estado
    RETURN @codigo;
END;
