using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SincronizadorAplix_Nidux.models;

namespace SincronizadorAplix_Nidux
{
    public partial class Service1 : ServiceBase
    {
        //referencias
        LogsFile logsFile = new LogsFile();
        Nidux nidux = new Nidux();


        //variables globales
        string token = "";

        public Service1()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            logsFile.WriteLogs("Servicio de Sincronización Nidux Inicializado ( " + DateTime.Now + " )");

            //nota: este timer es el encargado de ejecutar el servicio
            timerEjecucion.Elapsed += new ElapsedEventHandler(EjecucionSincronizador);
#if (DEBUG)
            //timerEjecucion.Interval = 15000;
            timerEjecucion.Start();
#else
     timerEjecucion.Interval = Convert.ToInt32(ConfigurationSettings.AppSettings["Tiempo"].ToString());
#endif

            timerEjecucion.Start();
        }

        private async void EjecucionSincronizador(object source, ElapsedEventArgs e)
        {
            timerEjecucion.Enabled = false;
            timerEjecucion.Stop();
            try
            {
                logsFile.WriteLogs("Iniciando tareas de sincronizacion ( " + DateTime.Now + " )");
                //obtenemos los credenciales de la tienda
                Credencial credencial = await nidux.GetCredenciales();
                if (credencial.credenciales.Count > 0)
                {
                    //obtenemos el bearer token
                    token = await nidux.GetTokenNidux(credencial.credenciales[0].usuario, credencial.credenciales[0].contrasena, credencial.credenciales[0].storeId);

                    if (token != "")
                    {
                        //Consulto fecha
                        string fecha_consulta = nidux.ConsultoFecha2();

                        //Sincronizar articulos de Oracle a SQL
                        logsFile.WriteLogs("Sincronizacion de la Vista de Oracle -> Sincronizador:");
                        //nidux.GetArticulosOracle();

                        //Sincronizar articulos simple
                        logsFile.WriteLogs("Sincronizacion de articulos del Sincronizador -> Nidux:");
                        await nidux.PostArticulosNidux(token);

                        //Sincronizar Pedidos
                        logsFile.WriteLogs("Sincronizacion de Pedidos de Nidux -> Oracle:");
                        await nidux.SincronizarPedidosOracle(token);

                        //Actualizar fecha
                        await nidux.UpdateDate();
                        logsFile.WriteLogs("Finalizando tareas de sincronizacion ( " + DateTime.Now + " )");

                        //ACTUALIZO LA FECHA LUEGO DE EJECUTAR
                        nidux.actualizar_fechaV2(fecha_consulta);
                    }
                    else
                    {
                        logsFile.WriteLogs("Fallo en obtener el token de Nidux, revisar los datos que se están enviando al endpoint de Nidux, hora fallo: " + DateTime.Now);
                    }
                }
                else
                {
                    logsFile.WriteLogs("Error en obtener credenciales de la Tienda, estos vienen vacios por favor verificar el endpoint 'obtener_credenciales', hora fallo: " + DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                logsFile.WriteLogs("Mensaje de error: " + ex.Message);
            }
            finally
            {
                timerEjecucion.Enabled = true;
                timerEjecucion.Start();
            }
        }

        protected override void OnStop()
        {
            logsFile.WriteLogs("Servicio de Sincronización Finalizado ( " + DateTime.Now + " )");
            timerEjecucion.Stop();
        }

        private void timerEjecucion_Elapsed(object sender, ElapsedEventArgs e)
        {

        }
    }
}
