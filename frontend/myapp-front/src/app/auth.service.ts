import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})

export class AuthService {

    private http = inject(HttpClient);

    private apiUrl = 'http://localhost:5029/api/auth';

    login(provider: string): Observable<any> {
        const payload = { provider: provider };

        return this.http.post(`${this.apiUrl}/login`, payload);
    }
}
    