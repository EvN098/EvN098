
--tablas relacionadas para saldo:
select * from farmSaldo
select * from farmAlmacen
select * from FactCatalogoBienesInsumos
select * from farmTipoSalidaBienInsumo
select * from farmSaldo



/****************************************************************/
--Para revisión de archivos que llegan a SIGH de MACROSCOPIA
select * from LabMovimientoLaboratorio where IdMovimiento=903435 -- para obtener idordemn
select * from LabResultado where idOrden=3708095 --para obtener idlabresultado
select * from ArchivosCab where idCampo1=18042 --para obtener idarchivo
select * from ArchivosDet where idArchivo=413132 --aqui se visualiza los archivos
--acceso del medico
SELECT * FROM Empleados WHERE IdEmpleado=3468

/****************************************************************/
--para productos:
FactCatalogoBienesInsumos


/****************************************************************/
--updates para revertir anulacion de citas con pago efectivo
select * from Atenciones where IdCuentaAtencion = 1669819
update Atenciones set idEstadoAtencion = 1 where IdCuentaAtencion = 1669819

select * from citas where IdAtencion = 1668589
update citas set idEstado = 1 where IdAtencion = 1668589

select * from FacturacionCuentasAtencion where IdCuentaAtencion = 1669819 
update FacturacionCuentasAtencion set IdEstado = 1  where IdCuentaAtencion = 1669819 

select * from PagoEfectivo where idCuentaAtencion = 1669819
update PagoEfectivo set FechaPagoCIP = '2022-11-15 08:39:11.000', CodigoTransaccion = 9999999 , idEstadoCIP = 2 , Observaciones = 'manual cod transaccion', idEstado = 1 where idCuentaAtencion = 1669819

select * from FactOrdenServicio where IdCuentaAtencion = 1669819
update FactOrdenServicio set IdEstadoFacturacion= 1 where IdCuentaAtencion = 1669819

select * from FactOrdenServicioPagos where idOrden = 3699430
update FactOrdenServicioPagos set IdEstadoFacturacion = 1 where idOrden = 3699430

/****************************************************************/
--Proceso para actualizar el estado de citas:
select * from citas where idEstadoColaCitas = 3 

update citas set idEstadoColaCitas where IdAtencion = 
--idatencion conseguir:
select * from atenciones where idcuentaatencion -> la primera fila de la consulta es el idatencion

/****************************************************************/
