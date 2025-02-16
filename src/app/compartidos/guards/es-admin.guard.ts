import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { SeguridadService } from './seguridad.service';

export const esAdminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
const segurridadService = inject(SeguridadService);


if(segurridadService.obtenerRol() === 'admin'){
  return true;
}

router.navigate(['/login']);
return true;
};
