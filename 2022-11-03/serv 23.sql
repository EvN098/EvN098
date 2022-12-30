use SIGH

;with cte as (  
     select 
		a.IdAtencion,
		a.IdPaciente,
		a.IdCuentaAtencion,
		a.IdServicioIngreso,
		s.Nombre as servicio,
		s.IdTipoServicio
      from dbo.Atenciones (nolock) a  
		left join dbo.FacturacionCuentasAtencion (nolock) fca on a.IdCuentaAtencion=fca.IdCuentaAtencion  
		left join dbo.Servicios (nolock) s on A.IdServicioEgreso=s.IdServicio  
		where a.idTipoServicio in (3) and a.esPacienteExterno<>1  
		and 
		(  
			--(a.FechaEgreso is null and fca.IdEstado in (1,12) and s.IndEgresoFisico is null)
			(a.FechaEgreso is null and fca.IdEstado in (1,12) and a.IndEgresoFisico is null)
			or  
			--(a.FechaEgresoFisico is null and fca.IdEstado in (1,10,12) and s.IndEgresoFisico =1)
			(a.FechaEgresoFisico is null and fca.IdEstado in (1,10,12) and a.IndEgresoFisico =1)
		)    
  )

select * from cte