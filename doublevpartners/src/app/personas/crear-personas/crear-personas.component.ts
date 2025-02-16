import { Component , inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validator, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import {
  MatSnackBar,
  MatSnackBarAction,
  MatSnackBarActions,
  MatSnackBarLabel,
  MatSnackBarRef,
} from '@angular/material/snack-bar';
import { HttpErrorResponse } from '@angular/common/http';
import { FormularioPersonaComponent } from '../formulario-persona/formulario-persona.component';
import { PersonasService } from '../personas.service';
import { PersonaCreacionDTO } from '../personas';
@Component({
  selector: 'app-crear-personas',
  standalone: true,
  imports: [MatButtonModule, FormularioPersonaComponent],
  templateUrl: './crear-personas.component.html',
  styleUrl: './crear-personas.component.css'
})
export class CrearPersonasComponent {
  router = inject(Router);
  personaService= inject(PersonasService);
  private _snackBar = inject(MatSnackBar);

 guardarCambios(persona: PersonaCreacionDTO){
   console.log('Insertar persona', persona);
   this.personaService.crearPersona(persona).subscribe({
     next: (persona) =>{
        this.router.navigate(['/persona']);
        this.openSnackBar(persona.response.message);
     },
     error: (error:HttpErrorResponse) => {
       // Manejar el error 404 aquí
       if (error.error === 404) {
         
         this.openSnackBar(error.message)
       } else {
        this.openSnackBar('Ocurrió un error desconocido');
       }
     }
   });
   //console.log(this.form.value);
 }

 openSnackBar(message: string) {
   this._snackBar.open(message, "",{
     duration: 4 * 1000,
   });
 }


}
