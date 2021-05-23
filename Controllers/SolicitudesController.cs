using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using automotriz_webapi.Libs;
using automotriz_webapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace automotriz_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesController:ControllerBase
    {
        private readonly automotrizContext Db;

        public SolicitudesController(automotrizContext db)
        {
            this.Db = db;
        }

        private async Task<(Solicitude solicitud, decimal? ingresoAcumulable)> ObtenerSugerido(int porcentaje, Cliente cliente, Solicitude solicitud){
                var ingresoAcumulable = cliente.IngresosMensuales * porcentaje / 100;
                var planSugerido = await Db.PlanesFinanciamientos
                                    .Where(pl => pl.MinIngresoAcumulable <= ingresoAcumulable)
                                    .OrderByDescending(pl => pl.PrecioLimite)
                                    .ToListAsync();
                solicitud.IdCliente = cliente.Id;
                solicitud.Aprobado = true;
                solicitud.IdPlanFinanciamiento = ingresoAcumulable < 5000 ? 1 : planSugerido[0].Id;
                return (solicitud, ingresoAcumulable);
        }
        // pending
        /* Verificar cliente registrado contra plan */
        [HttpPost]
        [Route("cliente/validar_plan/{cliente}")]
        public async Task<IActionResult> Validar([FromRoute]int cliente){
            var lastSolicitud = await this.Db.Solicitudes
                                            .Include(sl => sl.IdPlanFinanciamientoNavigation)
                                            .LastOrDefaultAsync(sl => sl.IdCliente == cliente);
            if(lastSolicitud == null) return NotFound();
            return Ok(new {
                plan = lastSolicitud.IdPlanFinanciamiento,
                msg = $"El cliente actualmente cuenta con el plan ${lastSolicitud.IdPlanFinanciamientoNavigation.Descripcion}"
            });
        }
        /* Crear nueva solicitud, ejecutar algoritmo de asignacion de plan sugerido. */
        [HttpPost]
        [Route("cliente/{clienteId}")]
        public async Task<IActionResult> NuevaSolicitud([FromRoute]int clienteId){
            var solicitud  = new Solicitude();
            var cliente = await this.Db.Clientes
                                    .Include(cl => cl.IdEstadoCivilNavigation)
                                    .Include(cl => cl.Hijos)
                                    .Include(cl => cl.Solicitudes)
                                    .FirstOrDefaultAsync(cl => cl.Id == clienteId);
            // Do algorithm for sugeridos
            if(cliente.IdEstadoCivilNavigation.Id == 1){
                // Está soltero.
                var sugeridoResponse = await ObtenerSugerido(80, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                var solicitudFromDb = await this.Db.Solicitudes
                                                .Include(sl => sl.IdClienteNavigation)
                                                .Include(sl => sl.IdPlanFinanciamientoNavigation)
                                                .FirstOrDefaultAsync(sl => sl.Id == solicitudResponse.Entity.Id);
                return Ok(new {
                    id_solicitud = solicitudFromDb.Id,
                    fecha_solicitud = solicitudFromDb.Fecha,
                    cliente = new {
                        id_cliente = solicitudFromDb.IdClienteNavigation.Id,
                        curp = solicitudFromDb.IdClienteNavigation.Curp,
                        nombre_completo = solicitudFromDb.IdClienteNavigation.NombreCompleto,
                        ingreso_acumulable_actual = sugeridoResponse.ingresoAcumulable,
                    },
                    resultados = new {
                        aprobado = solicitudFromDb.Aprobado,
                        plan_sugerido = new {
                            id_plan = solicitudFromDb.IdPlanFinanciamientoNavigation.Id,
                            descripcion = solicitudFromDb.IdPlanFinanciamientoNavigation.Descripcion,
                            precio_inicial = solicitudFromDb.IdPlanFinanciamientoNavigation.PrecioInicial,
                            precio_limite = solicitudFromDb.IdPlanFinanciamientoNavigation.PrecioLimite,
                            min_ingreso_acumulable = solicitudFromDb.IdPlanFinanciamientoNavigation.MinIngresoAcumulable
                        }
                    }
                });
            }else if (cliente.IdEstadoCivilNavigation.Id == 2 && cliente.Hijos.Count == 0){
                // Está casado y no tiene hijos.
                var sugeridoResponse = await ObtenerSugerido(70, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                var solicitudFromDb = await this.Db.Solicitudes
                                                .Include(sl => sl.IdClienteNavigation)
                                                .Include(sl => sl.IdPlanFinanciamientoNavigation)
                                                .FirstOrDefaultAsync(sl => sl.Id == solicitudResponse.Entity.Id);
                return Ok(new {
                    id_solicitud = solicitudFromDb.Id,
                    fecha_solicitud = solicitudFromDb.Fecha,
                    cliente = new {
                        id_cliente = solicitudFromDb.IdClienteNavigation.Id,
                        curp = solicitudFromDb.IdClienteNavigation.Curp,
                        nombre_completo = solicitudFromDb.IdClienteNavigation.NombreCompleto,
                        ingreso_acumulable_actual = sugeridoResponse.ingresoAcumulable,
                    },
                    resultados = new {
                        aprobado = solicitudFromDb.Aprobado,
                        plan_sugerido = new {
                            id_plan = solicitudFromDb.IdPlanFinanciamientoNavigation.Id,
                            descripcion = solicitudFromDb.IdPlanFinanciamientoNavigation.Descripcion,
                            precio_inicial = solicitudFromDb.IdPlanFinanciamientoNavigation.PrecioInicial,
                            precio_limite = solicitudFromDb.IdPlanFinanciamientoNavigation.PrecioLimite,
                            min_ingreso_acumulable = solicitudFromDb.IdPlanFinanciamientoNavigation.MinIngresoAcumulable
                        }
                    }
                });
            }else if (cliente.IdEstadoCivilNavigation.Id == 2 && cliente.Hijos.Count == 1){
                // Está casado y tiene 1 hijo.
                var sugeridoResponse = await ObtenerSugerido(60, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                var solicitudFromDb = await this.Db.Solicitudes
                                                .Include(sl => sl.IdClienteNavigation)
                                                .Include(sl => sl.IdPlanFinanciamientoNavigation)
                                                .FirstOrDefaultAsync(sl => sl.Id == solicitudResponse.Entity.Id);
                return Ok(new {
                    id_solicitud = solicitudFromDb.Id,
                    fecha_solicitud = solicitudFromDb.Fecha,
                    cliente = new {
                        id_cliente = solicitudFromDb.IdClienteNavigation.Id,
                        curp = solicitudFromDb.IdClienteNavigation.Curp,
                        nombre_completo = solicitudFromDb.IdClienteNavigation.NombreCompleto,
                        ingreso_acumulable_actual = sugeridoResponse.ingresoAcumulable,
                    },
                    resultados = new {
                        aprobado = solicitudFromDb.Aprobado,
                        plan_sugerido = new {
                            id_plan = solicitudFromDb.IdPlanFinanciamientoNavigation.Id,
                            descripcion = solicitudFromDb.IdPlanFinanciamientoNavigation.Descripcion,
                            precio_inicial = solicitudFromDb.IdPlanFinanciamientoNavigation.PrecioInicial,
                            precio_limite = solicitudFromDb.IdPlanFinanciamientoNavigation.PrecioLimite,
                            min_ingreso_acumulable = solicitudFromDb.IdPlanFinanciamientoNavigation.MinIngresoAcumulable
                        }
                    }
                });
            }else if (cliente.IdEstadoCivilNavigation.Id == 2 && cliente.Hijos.Count == 2){
                // Está casado y tiene 2 hijos.
                var sugeridoResponse = await ObtenerSugerido(55, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                var solicitudFromDb = await this.Db.Solicitudes
                                                .Include(sl => sl.IdClienteNavigation)
                                                .Include(sl => sl.IdPlanFinanciamientoNavigation)
                                                .FirstOrDefaultAsync(sl => sl.Id == solicitudResponse.Entity.Id);
                return Ok(new {
                    id_solicitud = solicitudFromDb.Id,
                    fecha_solicitud = solicitudFromDb.Fecha,
                    cliente = new {
                        id_cliente = solicitudFromDb.IdClienteNavigation.Id,
                        curp = solicitudFromDb.IdClienteNavigation.Curp,
                        nombre_completo = solicitudFromDb.IdClienteNavigation.NombreCompleto,
                        ingreso_acumulable_actual = sugeridoResponse.ingresoAcumulable,
                    },
                    resultados = new {
                        aprobado = solicitudFromDb.Aprobado,
                        plan_sugerido = new {
                            id_plan = solicitudFromDb.IdPlanFinanciamientoNavigation.Id,
                            descripcion = solicitudFromDb.IdPlanFinanciamientoNavigation.Descripcion,
                            precio_inicial = solicitudFromDb.IdPlanFinanciamientoNavigation.PrecioInicial,
                            precio_limite = solicitudFromDb.IdPlanFinanciamientoNavigation.PrecioLimite,
                            min_ingreso_acumulable = solicitudFromDb.IdPlanFinanciamientoNavigation.MinIngresoAcumulable
                        }
                    }
                });
            }else if (cliente.IdEstadoCivilNavigation.Id == 2 && cliente.Hijos.Count >= 3){
                // Está casado y tiene 3 hijos o más.
                var sugeridoResponse = await ObtenerSugerido(50, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                var solicitudFromDb = await this.Db.Solicitudes
                                                .Include(sl => sl.IdClienteNavigation)
                                                .Include(sl => sl.IdPlanFinanciamientoNavigation)
                                                .FirstOrDefaultAsync(sl => sl.Id == solicitudResponse.Entity.Id);
                return Ok(new {
                    id_solicitud = solicitudFromDb.Id,
                    fecha_solicitud = solicitudFromDb.Fecha,
                    cliente = new {
                        id_cliente = solicitudFromDb.IdClienteNavigation.Id,
                        curp = solicitudFromDb.IdClienteNavigation.Curp,
                        nombre_completo = solicitudFromDb.IdClienteNavigation.NombreCompleto,
                        ingreso_acumulable_actual = sugeridoResponse.ingresoAcumulable,
                    },
                    resultados = new {
                        aprobado = solicitudFromDb.Aprobado,
                        plan_sugerido = new {
                            id_plan = solicitudFromDb.IdPlanFinanciamientoNavigation.Id,
                            descripcion = solicitudFromDb.IdPlanFinanciamientoNavigation.Descripcion,
                            precio_inicial = solicitudFromDb.IdPlanFinanciamientoNavigation.PrecioInicial,
                            precio_limite = solicitudFromDb.IdPlanFinanciamientoNavigation.PrecioLimite,
                            min_ingreso_acumulable = solicitudFromDb.IdPlanFinanciamientoNavigation.MinIngresoAcumulable
                        }
                    }
                });
            }
            
            return NotFound();
            
        }

        [HttpPost]
        [Route("financiar")]
        public async Task<IActionResult> FinanciarAutomovil([FromBody]FinanciamientoModel financiamientoData){
            var plan = await Db.PlanesFinanciamientos.FirstOrDefaultAsync(pl => pl.Id == financiamientoData.Id_plan );
            var automovil = await Db.Autos
                                    .Include(au => au.IdModeloNavigation)
                                    .Include(au => au.IdModeloNavigation.IdMarcaNavigation)
                                    .FirstOrDefaultAsync(au => au.Id == financiamientoData.Id_automovil);

            var encanche = automovil.ValorComecial * 20 / 100;
            var cantidadFinanciar = (automovil.ValorComecial - encanche);
            var cantidadFinanciarProcessed = cantidadFinanciar + cantidadFinanciar * 35 / 100;
            var mensualidadProcessed = cantidadFinanciarProcessed / financiamientoData.Meses;

            return Ok(new {
                valor_del_auto = automovil.ValorComecial,
                enganche = encanche,
                cantidad_a_financiar = cantidadFinanciarProcessed,
                meses = financiamientoData.Meses,
                mensualidad = mensualidadProcessed,
                automovil
            });
        }

        [HttpPost]
        [Route("encrypt")]
        public async Task<ActionResult<object>> Encriptar([FromBody]EncryptionModel data){
            var algorithm = new TripleDesLib(data.Clave);
            var client = await this.Db.Clientes
                                .FirstOrDefaultAsync(cl => cl.Id == @data.ClienteId);
            if(client == null) return NotFound(new { msg = "El cliente solicitado no se encontro."});

            var encryptData = await this.Db.Encriptaciones
                                            .FirstOrDefaultAsync(crypt => crypt.IdCliente == @data.ClienteId);
            // Create encryptdata;
            if(encryptData == null){
                var result = await this.Db.Encriptaciones.AddAsync(new Encriptacione{
                        IdCliente = client.Id,
                        IsEncripted = false
                    });
                await this.Db.SaveChangesAsync();
                encryptData = result.Entity; 
            }

            if(encryptData.IsEncripted){
                // Decriptar.
                client.NombreCompleto = algorithm.Decrypt(client.NombreCompleto);
                client.Curp = algorithm.Decrypt(client.Curp);
                client.Domicilio = algorithm.Decrypt(client.Domicilio);
                // client.IngresosMensuales = Convert.ToDecimal( algorithm.Decrypt(client.IngresosMensuales.ToString()) );
                // client.Edad = Convert.ToByte( algorithm.Decrypt(client.Edad.ToString()) );
                // client.FechaNacimiento = Convert.ToDateTime( algorithm.Decrypt(client.FechaNacimiento.ToString()) );
                encryptData.IsEncripted = false;
                await this.Db.SaveChangesAsync();
                return Ok(new {
                    ok = true,
                    msg = $"Informacion del cliente {client.NombreCompleto} decriptada correctamente."
                });
            }else{
                // Encriptar
                client.NombreCompleto = algorithm.Encrypt(client.NombreCompleto);
                client.Curp = algorithm.Encrypt(client.Curp);
                client.Domicilio = algorithm.Encrypt(client.Domicilio);
                // client.IngresosMensuales = decimal.Parse( algorithm.Encrypt(client.IngresosMensuales.ToString()), CultureInfo.InvariantCulture );
                // client.Edad = byte.Parse( algorithm.Encrypt(client.Edad.ToString()), CultureInfo.InvariantCulture );
                // client.FechaNacimiento = Convert.ToDateTime( algorithm.Encrypt(client.FechaNacimiento.ToString()) );
                encryptData.IsEncripted = true;
                await this.Db.SaveChangesAsync();
                return Ok(new {
                    ok = true,
                    msg = $"Informacion del cliente {client.NombreCompleto} encriptada correctamente."
                });
            }
        }

        [HttpGet]
        [Route("is_encripted/{idCliente}")]
        public async Task<ActionResult<object>> HasEncryptedData([FromRoute]int idCliente){
            var isEncrypted = await this.Db.Encriptaciones.FirstOrDefaultAsync(enc => enc.IdCliente == idCliente);
            if(isEncrypted == null) return NotFound(new {ok = false, msg = "No hay rastro de este cliente en la base de datos."});

            return isEncrypted.IsEncripted ? new {
                ok = true,
                is_encripted = true,
                msg = "ENCRIPTADO"
            } 
            : 
            new  {
                ok = true,
                is_encripted = false,
                msg = "NO ENCRIPTADO"
            };
        }
    }
}