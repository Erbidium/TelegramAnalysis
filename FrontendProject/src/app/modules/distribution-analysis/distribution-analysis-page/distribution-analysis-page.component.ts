import { formatDate } from '@angular/common';
import { Component, Inject, LOCALE_ID } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BaseComponent } from '@core/base/base.component';
import { SpreadGraphItem } from '@core/models/spread-graph-item';
import { NotificationService } from '@core/services/notification.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { EChartsOption } from 'echarts';
import { DistributionAnalysisService } from "@core/services/distribution-analysis.service";

@Component({
    selector: 'app-distribution-analysis-page',
    templateUrl: './distribution-analysis-page.component.html',
    styleUrls: ['./distribution-analysis-page.component.sass'],
})
export class DistributionAnalysisPageComponent extends BaseComponent {
    // @ts-ignore
    options: EChartsOption;

    public searchForm = new FormGroup(
        {
            postText: new FormControl(
                '',
                [
                    Validators.required,
                ],
            ),
        },
        {
            updateOn: 'blur',
        },
    );

    constructor(
        private analysisService: DistributionAnalysisService,
        @Inject(LOCALE_ID) private locale: string,
        private notifications: NotificationService,
        private spinnerService: SpinnerOverlayService,
    ) {
        super();
    }

    loadSpreadGraph() {
        const post = this.searchForm.value.postText!;

        this.spinnerService.show();
        this.analysisService.getPostDistributionGraph(post)
            .pipe(this.untilThis)
            .subscribe(
                graphItems => {
                    this.spinnerService.hide();
                    console.log(graphItems);

                    const nodes = this.createNodes(graphItems);
                    const edges = this.createEdges(graphItems, nodes);

                    this.setupGraphVisualization(nodes, edges);
                },
                error => {
                    this.spinnerService.hide();

                    console.log(error);
                    this.notifications.showWarningMessage(`Сталася помилка!
                    Ваше повідомлення не знайдено серед повідомлень, які були отримані в результаті парсингу`);
                },
            );
    }

    createEdges(nodeItems: SpreadGraphItem[], nodes: { id: string, name: string }[]) {
        const edges = [];

        for (let i = 0; i < nodes.length; i++) {
            const nodeItem = nodeItems[i];
            const node = nodes[i];

            const parentNodeId = nodeItem.root_id;

            if (parentNodeId != null) {
                const parentNodeIndex = nodeItems.findIndex(n => n.post_id === parentNodeId);

                if (parentNodeIndex >= 0) {
                    edges.push([+nodes[parentNodeIndex].id, +node.id]);
                }
            }
        }

        return edges;
    }

    createNodes(nodeItems: SpreadGraphItem[]) {
        const nodes = [];

        for (let i = 0; i < nodeItems.length; i++) {
            nodes.push({
                id: `${i}`,
                name: `Схожість: ${Math.round(nodeItems[i].similarity_with_wanted * 10000) / 100}%\n\n
                Створено: ${formatDate(nodeItems[i].created_at, 'short', this.locale)}\n\n
                Канал: ${nodeItems[i].channel_title}\n\n${nodeItems[i].text.substring(0, 100)}...`,
            });
        }

        return nodes;
    }

    setupGraphVisualization(nodes: { id: string, name: string }[], edges: number[][]) {
        this.options = {
            series: [
                {
                    type: 'graph',
                    layout: 'force',
                    animation: true,
                    animationDuration: 1500,
                    animationEasingUpdate: 'quinticInOut',
                    emphasis: {
                        lineStyle: {
                            width: 10,
                        },
                        focus: 'adjacency',
                    },
                    data: nodes,
                    width: '100%',
                    height: '100%',
                    lineStyle: {
                        curveness: 0.3,
                        opacity: 0.9,
                        width: 2,
                    },
                    itemStyle: {
                        borderColor: '#fff',
                        borderWidth: 1,
                        shadowBlur: 10,
                        shadowColor: 'rgba(0, 0, 0, 0.3)',
                    },
                    symbolSize: 60,
                    draggable: true,
                    edgeSymbol: ['circle', 'arrow'],
                    roam: true,
                    force: {
                        repulsion: 4000,
                        edgeLength: 70,
                        gravity: 0.2,
                    },
                    label: {
                        show: true,
                        rich: {
                            bold: {
                                fontWeight: 'bold',
                            },
                        },
                        position: 'right',
                        formatter: params => {
                            const labelText = params.name;
                            const similarityTextEnd = labelText.indexOf('\n');
                            const similarityStr = labelText.substring(0, similarityTextEnd);
                            const afterSimilarityStr = labelText.substring(similarityTextEnd);

                            return `{bold|${similarityStr}}${afterSimilarityStr}`;
                        },
                    },
                    labelLine: 'show',
                    edges: edges.map(e => ({
                        source: `${e[0]}`,
                        target: `${e[1]}`,
                    })),
                },
            ],
        };
    }
}
