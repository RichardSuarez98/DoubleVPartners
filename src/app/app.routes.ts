import { Routes } from '@angular/router';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { PersonasService } from './personas/personas.service';
import { IndicePersonasComponent } from './personas/indice-personas/indice-personas.component';
import { CrearPersonasComponent } from './personas/crear-personas/crear-personas.component';
import { EditarPersonasComponent } from './personas/editar-personas/editar-personas.component';
import { esAdminGuard } from './compartidos/guards/es-admin.guard';
import { LoginComponent } from './seguridad/login/login.component';
import { CrearPersonausuarioComponent } from './personas/crear-personausuario/crear-personausuario.component';

export const routes: Routes = [
    {path: '', component:LandingPageComponent},
    {path: 'registrar', component:CrearPersonausuarioComponent},
    {path: 'persona', component:IndicePersonasComponent, canActivate:[esAdminGuard]},
    {path: 'persona/crear', component:CrearPersonasComponent, canActivate:[esAdminGuard]},
    {path: 'persona/editar/:id', component:EditarPersonasComponent, canActivate:[esAdminGuard]},
    {path: 'login', component:LoginComponent},
    {path: '**' , redirectTo:''},
];

