import { AfterViewInit, Component, inject, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { PersonasDTO } from '../personas';
import { PersonasService } from '../personas.service';
import Swal from 'sweetalert2'
import { HttpErrorResponse } from '@angular/common/http';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-indice-personas',
  standalone: true,
  imports: [MatButtonModule,RouterLink,MatTableModule,MatPaginatorModule,MatFormFieldModule, MatInputModule],
  templateUrl: './indice-personas.component.html',
  styleUrl: './indice-personas.component.css'
})
export class IndicePersonasComponent {
  datasource: any

  columnasMostrar: string[] = ['idPersona', 'nombres','apellidos','numeroIdentificacion','email','tipoIdentificacion', 'Accion'];

  personaService= inject(PersonasService);
  listadoPersonas!: PersonasDTO[];

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.datasource.filter = filterValue.trim().toLowerCase();
  }

  constructor(){
    this.cargarListadoPersonas();
  }
  @ViewChild(MatPaginator) paginator!: MatPaginator;


  cargarListadoPersonas(){
    this.personaService.obtenerPersonas()
                .subscribe((resppuesta)  =>{
      this.listadoPersonas=resppuesta.data as PersonasDTO[];
      console.log(this.listadoPersonas);
      this.datasource = new MatTableDataSource(this.listadoPersonas);
      this.datasource.paginator = this.paginator;
      console.log(this.datasource +"datasource");
    });
  }


  
  borrar(idUnico:number){
    console.log("Este es el id a eliminar"+idUnico);
    Swal.fire({
      title: "¿Esta seguro de elimnar este registro?",
      text: "Esta accion es irreversible!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      cancelButtonText:'Cancelar',
      confirmButtonText: "Si, quiero eliminar!"
    }).then((result) => {
      if (result.isConfirmed) {
        this.personaService.eliminarPersona(idUnico).subscribe({
          
          next: (personaEliminar)=>{
          this.cargarListadoPersonas();
            Swal.fire({
              title: "Se elimino correctamente!",
              text: "Your file has been deleted.",
              icon: "success"
            })
          },
          error: (error:HttpErrorResponse)=>{
            if(error.status === 404){
              Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "Lo sentimos ocurrió un error al eliminar a la persona: "+error.statusText,
              });
            }else{
              Swal.fire({
                icon: "error",
                title: "Oops...",
                text: error.message,
              });
            }
          }


        });
      }
    });
  }



}
