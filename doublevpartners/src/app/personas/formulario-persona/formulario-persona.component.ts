import { Component , EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { PersonaCreacionDTO, PersonasDTO } from '../personas';

@Component({
  selector: 'app-formulario-persona',
  standalone: true,
  imports: [MatButtonModule,MatFormFieldModule, ReactiveFormsModule, MatInputModule,RouterLink],
  templateUrl: './formulario-persona.component.html',
  styleUrl: './formulario-persona.component.css'
})
export class FormularioPersonaComponent implements OnInit {
  ngOnInit(): void {
    if(this.modelo !== undefined){
      this.form.patchValue(this.modelo);
    }
  }

  @Output()
  posteoFormulario = new EventEmitter<PersonaCreacionDTO>();

  @Input() modelo?: PersonasDTO;



  private formBuilder = inject(FormBuilder);

  form = this.formBuilder.group({
    nombres: ['',{validators:[Validators.required]}],
    apellidos: ['',{validators:[Validators.required]}],
    numeroIdentificacion: ['',{validators:[Validators.required]}],
    email: ['',{validators:[Validators.required]}],
    tipoIdentificacion: ['',{validators:[Validators.required, Validators.maxLength(3)]}]
  });

  guardarCambios(){
    console.log(this.form.value);
    if(!this.form.valid){
      return;
    }
    const persona = this.form.value as PersonaCreacionDTO;
    this.posteoFormulario.emit(persona);
  }

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
  


}
