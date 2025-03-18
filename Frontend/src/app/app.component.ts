import { Component } from '@angular/core';
import {RouterLink, RouterOutlet} from '@angular/router';
import {HTTP_INTERCEPTORS} from '@angular/common/http';
import {Interceptor} from './core/interceptor';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  standalone: true,
  styleUrl: './app.component.css',
  providers:[{
    provide: HTTP_INTERCEPTORS,
    useClass: Interceptor,
    multi: true
  }]
})
export class AppComponent {
  title = 'Frontend';
}
