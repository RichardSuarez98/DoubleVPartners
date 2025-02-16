import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import {MatSelectModule} from '@angular/material/select';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import {MatButtonModule} from '@angular/material/button';
import { CredencialesUsuarioDTO } from '../seguridad';

@Component({
  selector: 'app-formulario-autenticacion',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule,ReactiveFormsModule
    ,RouterLink,MatButtonModule
  ],  //ReactiveFormModule
  templateUrl: './formulario-autenticacion.component.html',
  styleUrl: './formulario-autenticacion.component.css'
})
export class FormularioAutenticacionComponent {

  private formBuilder = inject(FormBuilder);

  form = this.formBuilder.group({
    usuario: ['', {validators: [Validators.required]}],
    password: ['', {validators: [Validators.required]}]
  });

  @Input({required:true})
  titulo!: string;

  @Output()
  posteoFormulario = new EventEmitter<CredencialesUsuarioDTO>();


  obtenerMensajeErrorUsuario(): string{
    let campo = this.form.controls.usuario;

    if(campo.hasError('required')){
      return 'El campo usuario es requerido';
    }

    return '';
  }

  obtenerMensajeErrorPassword(): string{
    let campo = this.form.controls.password;

    if(campo.hasError('required')){
      return 'El campo password es requerido';
    }

    return '';
  }

  guardarCambios(){
    if(!this.form.valid){
      return;
    }
    const credenciales = this.form.value as CredencialesUsuarioDTO;
    this.posteoFormulario.emit(credenciales);
    console.log(credenciales);
  }


}
