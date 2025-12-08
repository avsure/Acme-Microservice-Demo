import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../app.config';

export interface Product {
  id: string;
  name: string;
  price: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private readonly productUrl: string;

  constructor(
    private http: HttpClient,
    @Inject(API_BASE_URL) baseUrl: string
  ) {
    this.productUrl = `${baseUrl}/products/api/products`;
  }

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.productUrl);
  }

  getById(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.productUrl}/${id}`);
  }
}
