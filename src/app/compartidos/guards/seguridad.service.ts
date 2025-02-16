import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { CredencialesUsuarioDTO, RespuestaAutenticacionDTO } from '../../seguridad/seguridad';
import { ApiResponse } from '../Responsne/ApiResponse';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class SeguridadService {
private http = inject(HttpClient);
  private urlBase="https://localhost:7077/api/Usuarios/login";
  private readonly llaveToken='token';
  private readonly llaveExpiracion = 'llaveExpiracion';

 constructor() { }
 /*public obtenerRol():Observable<ApiResponse>{
   return this.http.get<ApiResponse>(`${this.urlBase}`);
 }*/
  router = inject(Router);

   obtenerRol(): string {
    const esAdmin = this.obtenerCampoJWT('esadmin');
    console.log("AUTENTICACION:",esAdmin);
    if (esAdmin) {
      return 'admin'
    } else {
      return '';
    }
  }

  obtenerToken(): string | null {
    return localStorage.getItem(this.llaveToken);
  }


  obtenerCampoJWT(campo: string): string {
    const token = localStorage.getItem(this.llaveToken);
    if (!token) { return '' }
    var dataToken = JSON.parse(atob(token.split('.')[1]))
    return dataToken[campo];
  }


 login(credenciales: CredencialesUsuarioDTO): Observable<RespuestaAutenticacionDTO> {
  return this.http.post<ApiResponse>(`${this.urlBase}`, credenciales).pipe(
    map((apiResponse: ApiResponse) => {
      // Verifica si la respuesta contiene los datos esperados
      if (apiResponse.data && apiResponse.data.token) {
        this.guardarToken(apiResponse.data);
        return {
          token: apiResponse.data.token,
          expiracion: apiResponse.data.expiracion
        } as RespuestaAutenticacionDTO;
      } else {
        throw new Error('Login Incorrecto');
      }
    }),
  );
}
  /*login(credenciales: CredencialesUsuarioDTO): Observable<RespuestaAutenticacionDTO>{
    return this.http.post<ApiResponse>(`${this.urlBase}`,credenciales).pipe(
        map((apiResponse: ApiResponse) => {
          // Extrae la primera persona del array "data"
          if (apiResponse.data && apiResponse.data.length > 0) {
            console.log("pues"+apiResponse.data);
            return apiResponse.data[0]; // Devuelve la primera persona
          } else {
            throw new Error('No se encontró la persona solicitada');
          }
        })
      );
   /* .pipe(
      tap(respuestaAutenticacion => this.guardarToken(respuestaAutenticacion))
    );
  }*/

  guardarToken(respuestaAutenticacion: RespuestaAutenticacionDTO){
    console.log("GUARDAR TOKEN: ",respuestaAutenticacion);
    localStorage.setItem(this.llaveToken,respuestaAutenticacion.token);
    localStorage.setItem(this.llaveExpiracion,respuestaAutenticacion.expiracion.toString());
  }

  estaLogueado(): boolean{
    const token = localStorage.getItem(this.llaveToken);

    if(!token){
      return false;
    }
    const expiracion = localStorage.getItem(this.llaveExpiracion);
    const expiracionFecha= new Date(expiracion!);

    if(expiracionFecha <= new Date()){
        this.logout();
        return false;
    }
    return true;
  }

  logout(){
    localStorage.removeItem(this.llaveToken);
    localStorage.removeItem(this.llaveExpiracion);
    this.router.navigate(['/login']);
  }



}
