import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecommendationService, RecommendationDto } from '../../services/recommendation.service';

@Component({
  selector: 'app-recommendations',
  standalone: true,
  imports: [CommonModule],
  template: `
    <h2>Recommendations</h2>

    <div *ngIf="loading">Loading recommendations...</div>

    <ul *ngIf="!loading && recommendations?.length; else noData">
      <li *ngFor="let r of recommendations; trackBy: trackById">
        {{ r.message }}
      </li>
    </ul>

    <ng-template #noData>
      <div>No recommendations found.</div>
    </ng-template>
  `
})
export class RecommendationsComponent implements OnInit {

  recommendations: RecommendationDto[] = [];
  loading = true;

  constructor(private svc: RecommendationService) {}

  ngOnInit() {
    const defaultProductId = '11111111-1111-1111-1111-111111111111'; // ⭐ change based on your real product

    this.svc.getForProduct(defaultProductId).subscribe({
      next: (data) => {
        console.log("Recommendations received:", data);
        this.recommendations = data;
        this.loading = false;
      },
      error: (err) => {
        console.error("Error loading recommendations:", err);
        this.loading = false;
      }
    });
  }

  trackById(index: number, item: RecommendationDto) {
    return item.id;
  }
}
