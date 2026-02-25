import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from '../environments/environment';

@Injectable({
    providedIn: 'root'
})

export class AuthService {

    private http = inject(HttpClient);

    private apiUrl = environment.apiUrl;

    login(provider: string): Observable<any> {
        const payload = { provider: provider };

        return this.http.post(`${this.apiUrl}/login`, payload);
    }
}
    