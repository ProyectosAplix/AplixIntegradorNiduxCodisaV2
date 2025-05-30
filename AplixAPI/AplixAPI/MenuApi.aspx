<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuApi.aspx.cs" Inherits="AplixAPI.MenuApi" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
    <title>API APLIX</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="grid">
            <div>
                <h3>Obtener Credenciales para Conexion (GET)</h3>
                <h4>Método que obtiene los credenciales para la conexion a API de Terceros</h4>
                <a href="http://localhost:50761/api/obtener_credenciales">Ver Método</a>
            </div>
            <div>
                <h3>Obtener todos los Artículos Editado Recientemente (GET)</h3>
                <h4>Método que obtiene todos los artículos que han sido editados recientemente para actualiozarlo en Nidux</h4>
                <a href="http://localhost:50761/api/actualizar_articulos_editados_simple">Ver Método</a>
            </div>
            <div>
                <h3>Actualizar ID Nidux en Artículos (PUT)</h3>
                <h4>Método que actualiza el ID Nidux de los artículos en nuestras Tablas Propias que fueron agregados a Nidux</h4>
                <a href="http://localhost:50761/api/actualizar_id_articulos">Ver Método</a>
            </div>
            <div>
                <h3>Obtener Cantidad Disponible (GET)</h3>
                <h4>Método que obtiene la cantidad disponible de todos los artículos que se hayan editado recientemente</h4>
                <a href="http://localhost:50761/api/actualizar_cantidad">Ver Método</a>
            </div>
            <div>
                <h3>Obtener Precio Disponible (GET)</h3>
                <h4>Método que obtiene el precio disponible de todos los artículos que se hayan editado recientemente</h4>
                <a href="http://localhost:50761/api/actualizar_precios">Ver Método</a>
            </div>
            <div>
                <h3>Obtener Artículos Padre (GET)</h3>
                <h4>Método que obtiene todos los artículos padre que se hayan editado recientemente</h4>
                <a href="http://localhost:50761/api/obtener_articulos_padres">Ver Método</a>
            </div>
            <div>
                <h3>Obtener Atributos de un Artículo (GET)</h3>
                <h4>Método que obtiene todos los atributos de los artículos padres que se hayan editado recientemente</h4>
                <a href="http://localhost:50761/api/agregar_atributos_articulos">Ver Método</a>
            </div>
            <div>
                <h3>Obtener Artículos Hijos (GET)</h3>
                <h4>Método que obtiene todos los artículos hijos que se hayan editado recientemente</h4>
                <a href="http://localhost:50761/api/agregar_articulo_hijo">Ver Método</a>
            </div>
            <div>
                <h3>Obtener Artículos Eliminados (GET)</h3>
                <h4>Método que obtiene todos los artículos que ya no se quieren sincronizar y eliminar de Nidux</h4>
                <a href="http://localhost:50761/api/eliminar_articulos">Ver Método</a>
            </div>
            <div>
                <h3>Obtener Artículos Eliminados Padres (GET)</h3>
                <h4>Método que obtiene todos los artículos hijos que han sido ingresados como padres y se tienen que eliminar de Nidux como padres</h4>
                <a href="http://localhost:50761/api/eliminar_articulos_padres">Ver Método</a>
            </div>
            <div>
                <h3>Ingresar Atributos (POST)</h3>
                <h4>Método que obtiene de la Tienda de Nidux todos los Atributos para ingresarlos a Tablas Propias</h4>
                <a href="http://localhost:50761/api/insertar_atributos">Ver Método</a>
            </div>
            <div>
                <h3>Eliminar Valores del Atributos (DELETE)</h3>
                <h4>Método que elimina todos los valores de los atributos en nuestras Tablas Propias</h4>
                <a href="http://localhost:50761/api/eliminar_valores_atributos">Ver Método</a>
            </div>
            <div>
                <h3>Ingresar valores de Variaciones (POST)</h3>
                <h4>Método que obtiene de la Tienda Nidux todos lo valores de los Atributos para ingresarlos a Tablas Propias</h4>
                <a href="http://localhost:50761/api/insertar_valores_atributos">Ver Método</a>
            </div>
            <div>
                <h3>Insertar Marcas de Nidux (POST)</h3>
                <h4>Método que obtiene e inserta todos las Marcas disponibles en Nidux a nuestras Tablas Propias</h4>
                <a href="http://localhost:50761/api/insertar_marcas">Ver Método</a>
            </div>
            <div>
                <h3>Obtener Marcas Editadas Recientemente (GET)</h3>
                <h4>Método que obtiene todos las marcas editadas para actualizarlas en Nidux</h4>
                <a href="http://localhost:50761/api/actualizar_marcas_simple">Ver Método</a>
            </div>
            <div>
                <h3>Insertar Categorías de Nidux (POST)</h3>
                <h4>Método que obtiene e inserta todos las Categorías disponibles en Nidux a nuestras Tablas Propias</h4>
                <a href="http://localhost:50761/api/insertar_categorias">Ver Método</a>
            </div>
            <div>
                <h3>Obtener Categorías Editadas Recientemente (GET)</h3>
                <h4>Método que obtiene todos las categorías editadas para actualizarlas en Nidux</h4>
                <a href="http://localhost:50761/api/actualizar_categorias_nidux_simple">Ver Método</a>
            </div>
            <div>
                <h3>Agregar Pedidos de Nidux (POST)</h3>
                <h4>Método que obtiene los pedidos de la Tienda de Nidux para ingresarlos en nuestras Tablas Propias </h4>
                <a href="http://localhost:50761/api/insertar_pedidos">Ver Método</a>
            </div>
            <div>
                <h3>Obtener Estado de Pedidos (GET)</h3>
                <h4>Método que obtiene los pedidos que se han ingresado en Softland para actualizar el estado de la orden en Nidux</h4>
                <a href="http://localhost:50761/api/actualizar_estado_pedido">Ver Método</a>
            </div>
            <div>
                <h3>Actualizar Fecha de Consulta (GET)</h3>
                <h4>Método que actualiza la fecha de consulta de los registro de actualización y creación</h4>
                <a href="http://localhost:50761/api/actualizar_fecha">Ver Método</a>
            </div>
        </div>
    </form>
</body>
</html>

<style>
   body{
     background: rgb(247, 247, 247);
     padding: 1em;
   }

   .grid{
     display: grid;
     grid-template-columns: 1fr 1fr 1fr;
     gap: 1em;
   }

   h3{
     font-size: 18px;
     padding: 10px;
     margin-bottom: -5px;
   }

   h4{
     font-size: 14px;
     padding: 10px;
     margin-bottom: -5px;
   }

   .grid div{
     text-align: center;
     background: #FFF;
     border-radius: 5px;
     -webkit-box-shadow: 6px 9px 19px -10px rgba(107,107,107,1);
     -moz-box-shadow: 6px 9px 19px -10px rgba(107,107,107,1);
     box-shadow: 6px 9px 19px -10px rgba(107,107,107,1);
   }
</style>
