import { Component, inject } from '@angular/core';
import { SeguridadService } from '../../compartidos/guards/seguridad.service';
import { Router } from '@angular/router';
import { CredencialesUsuarioDTO, RespuestaAutenticacionDTO } from '../seguridad';
import { FormularioAutenticacionComponent } from "../formulario-autenticacion/formulario-autenticacion.component";
import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../../compartidos/Responsne/ApiResponse';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormularioAutenticacionComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  seguridadService = inject(SeguridadService);
  router = inject(Router);

  loguear(credenciales: CredencialesUsuarioDTO){
    console.log("credenciales son"+ credenciales);
    this.seguridadService.login(credenciales)
    .subscribe({
      next: (resp:RespuestaAutenticacionDTO) =>{
        console.log("Holaa LOGUEAR: "+resp.token);
        this.router.navigate(['']);
      },
      error: (err: HttpErrorResponse) =>{
        Swal.fire({
          title: err,
          icon: "warning",
          draggable: true
        });
        console.log("ERROR: "+err);
      }
    });
  }



}
