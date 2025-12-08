import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RECOMMENDATION_API_BASE_URL } from '../app.config';

export interface RecommendationDto {
  id: string;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class RecommendationService {

  private readonly recommendationUrl: string;

  constructor(
    private http: HttpClient,
    @Inject(RECOMMENDATION_API_BASE_URL) recommendationBaseUrl: string
  ) {
    this.recommendationUrl = `${recommendationBaseUrl}/recommendations/api/recommendations`;
  }

  getForProduct(productId: string): Observable<RecommendationDto[]> {
    return this.http.get<RecommendationDto[]>(`${this.recommendationUrl}/${productId}`);
  }

  create(productId: string, message: string): Observable<RecommendationDto> {
    return this.http.post<RecommendationDto>(this.recommendationUrl, { productId, message });
  }
}
