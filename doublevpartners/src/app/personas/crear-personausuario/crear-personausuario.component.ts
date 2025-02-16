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
import { PersonasService } from '../personas.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { PersonaCreacionDTO, PersonasDTO, PersonaUsuarioCreacionDTO } from '../personas';

@Component({
  selector: 'app-crear-personausuario',
  standalone: true,
  imports: [MatButtonModule,MatButtonModule,MatFormFieldModule, ReactiveFormsModule, MatInputModule,RouterLink],
  templateUrl: './crear-personausuario.component.html',
  styleUrl: './crear-personausuario.component.css'
})
export class CrearPersonausuarioComponent{
  router = inject(Router);
  personaService= inject(PersonasService);
  private _snackBar = inject(MatSnackBar);
  private formBuilder = inject(FormBuilder);

  form = this.formBuilder.group({
    nombres: ['',{validators:[Validators.required]}],
    apellidos: ['',{validators:[Validators.required]}],
    numeroIdentificacion: ['',{validators:[Validators.required]}],
    email: ['',{validators:[Validators.required]}],
    tipoIdentificacion: ['',{validators:[Validators.required, Validators.maxLength(3)]}],
    usuario: ['',{validators:[Validators.required]}],
    pass: ['',{validators:[Validators.required]}],
  });

  obteneMensajeErrorNombres(): string{
    let nombre = this.form.controls.nombres;
    if(nombre.hasError('required')){
      return "El campo Nombre es requerido";
    }else{
      return "";
    }
  }

  obteneMensajeErrorApellidos(): string{
    let nombre = this.form.controls.apellidos;
    if(nombre.hasError('required')){
      return "El campo Apellido es requerido";
    }else{
      return "";
    }
  }

  obteneMensajeErrorIdentificacion(): string{
    let nombre = this.form.controls.numeroIdentificacion;
    if(nombre.hasError('required')){
      return "El campo Identificación es requerido";
    }else{
      return "";
    }
  }

  obteneMensajeErrorEmail(): string{
    let nombre = this.form.controls.email;
    if(nombre.hasError('required')){
      return "El campo Email es requerido";
    }else{
      return "";
    }
  }



  obteneMensajeErrorTipoIdentificacion(): string {
    let tipoIdentificacionControl = this.form.controls.tipoIdentificacion;
  
    if (tipoIdentificacionControl.hasError('required')) {
      return "El campo Tipo de Identificación es requerido";
    } else if (tipoIdentificacionControl.hasError('maxlength')) {
      return "El campo Tipo de Identificación debe tener como máximo 3 caracteres";
    } else {
      return "";
    }
  }
  
  obteneMensajeErrorUsuario(): string {
    let tipoIdentificacionControl = this.form.controls.tipoIdentificacion;
  
    if (tipoIdentificacionControl.hasError('required')) {
      return "El campo Tipo de Usuario es requerido";}
    else {
      return "";
    }
  }

  obteneMensajeErrorPass(): string {
    let tipoIdentificacionControl = this.form.controls.tipoIdentificacion;
  
    if (tipoIdentificacionControl.hasError('required')) {
      return "El campo  Password es requerido";
    } else if (tipoIdentificacionControl.hasError('maxlength')) {
      return "El campo Password debe tener como máximo 3 caracteres";
    } else {
      return "";
    }
  }
  

 guardarCambios(){
  console.log(this.form.value);
  if(!this.form.valid){
    return;
  }
  const personaUsuario = this.form.value as PersonaUsuarioCreacionDTO;
   console.log('Insertar persona', personaUsuario);
   this.personaService.crearPersonaUsuario(personaUsuario).subscribe({
     next: (persona) =>{
        this.router.navigate(['/login']);
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
 }

 openSnackBar(message: string) {
   this._snackBar.open(message, "",{
     duration: 4 * 1000,
   });
  }






}