import { Component, inject, Input, numberAttribute, OnInit } from '@angular/core';
import { FormularioPersonaComponent } from '../formulario-persona/formulario-persona.component';
import { PersonasService } from '../personas.service';
import { Router } from '@angular/router';
import { PersonaCreacionDTO, PersonasDTO } from '../personas';
import { HttpErrorResponse, HttpHeaderResponse, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-editar-personas',
  standalone: true,
  imports: [FormularioPersonaComponent],
  templateUrl: './editar-personas.component.html',
  styleUrl: './editar-personas.component.css'
})
export class EditarPersonasComponent implements OnInit{

  ngOnInit(): void {
    this.obtenerPersonaPorId();
  }

  @Input({transform: numberAttribute})
  id! : number;
  persona?: PersonasDTO;
  personaService= inject(PersonasService);
  router = inject(Router);

  guardarCambios(persona: PersonaCreacionDTO){
     this.personaService.actualizarPersona(this.id,persona).subscribe({
      next: (response)=>{
        this.router.navigate(['/persona']);
        console.log(response);
      },
      error: (error:HttpErrorResponse)=>{
        console.log(error);
      }
     });
   }

   obtenerPersonaPorId(){
      this.personaService.ObtenerPersonaPorId(this.id).subscribe((response)=>{
        console.log(response);
        this.persona=response;
        console.log("0001: "+this.persona);
      });
   }



 

}
