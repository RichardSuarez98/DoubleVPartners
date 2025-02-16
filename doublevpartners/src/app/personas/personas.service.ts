import { HttpClient, HttpResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { PersonaCreacionDTO, PersonasDTO, PersonaUsuarioCreacionDTO } from './personas';
import { ApiResponse } from '../compartidos/Responsne/ApiResponse';

@Injectable({
  providedIn: 'root'
})
export class PersonasService {

  private http = inject(HttpClient);
  private urlBase="https://localhost:7077/api/Personas";
 constructor() { }
 public obtenerPersonas():Observable<ApiResponse>{
   return this.http.get<ApiResponse>(`${this.urlBase}`);
 }
 public eliminarPersona(personaId: number){
   return this.http.delete(`${this.urlBase}/${personaId}`);
 }
 
 public crearPersona(persona: PersonaCreacionDTO):Observable<ApiResponse>{
   return this.http.post<ApiResponse>(this.urlBase+"/registrar",persona);
 }

 public crearPersonaUsuario(persona: PersonaUsuarioCreacionDTO):Observable<ApiResponse>{
  return this.http.post<ApiResponse>(this.urlBase,persona);
}
 /*public ObtenerPersonaPorId(personaId: number):Observable<ApiResponse>{
   return this.http.get<ApiResponse>(`${this.urlBase}/${personaId}`);
 }*/
 public actualizarPersona(personaId:number,persona: PersonaCreacionDTO){
   return this.http.put(`${this.urlBase}/${personaId}`,persona)
 }

 public ObtenerPersonaPorId(personaId: number): Observable<PersonasDTO> {
  return this.http.get<ApiResponse>(`${this.urlBase}/${personaId}`).pipe(
    map((apiResponse: ApiResponse) => {
      // Extrae la primera persona del array "data"
      if (apiResponse.data && apiResponse.data.length > 0) {
        return apiResponse.data[0]; // Devuelve la primera persona
      } else {
        throw new Error('No se encontró la persona solicitada');
      }
    })
  );
}

 

}
