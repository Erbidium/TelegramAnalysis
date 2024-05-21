import { Component, Inject, LOCALE_ID, OnInit } from '@angular/core';
import { EChartsOption } from "echarts";
import { StatisticsService } from "@core/services/statistics.service";
import { BaseComponent } from "@core/base/base.component";
import { SpreadGraphItem } from "@core/models/spread-graph-item";
import { DatePipe, formatDate } from "@angular/common";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
})
export class AppComponent extends BaseComponent implements OnInit {
    // @ts-ignore
    options: EChartsOption;

    constructor(private statisticsService: StatisticsService, @Inject(LOCALE_ID) private locale: string)
    {
        super();
    }

    createNodes(nodeItems: SpreadGraphItem[]) {
        const nodes = [];
        for (let i = 0; i < nodeItems.length; i++) {
            nodes.push({
                id: i + '',
                name: `Схожість: ${Math.round(nodeItems[i].similarity * 10000) / 100}%\n\nСтворено: ${formatDate(nodeItems[i].created_at, 'short', this.locale)}\n\n${nodeItems[i].text.substring(0, 100)}...`
            });
        }
        return nodes;
    }

    getRandomInt(min: number, max: number) {
        // Use Math.floor to round down to the nearest whole number
        // Use Math.random() to generate a random decimal between 0 (inclusive) and 1 (exclusive)
        // Multiply by the range (max - min + 1) to cover the entire range
        // Add the minimum value to shift the range to [min, max]
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    createEdges(nodeItems: SpreadGraphItem[], nodes: {id: string, name: string}[]) {
        const edges = [];

        for (let i = 0; i < nodes.length; i++) {
            const nodeItem = nodeItems[i];
            const node = nodes[i];

            const parentNodeId = nodeItem.root_id;
            if (parentNodeId != null) {
                const parentNodeIndex = nodeItems.findIndex(i => i.post_id == parentNodeId);
                if (parentNodeIndex >= 0) {
                    edges.push([+nodes[parentNodeIndex].id, +node.id]);
                }
            }
        }
        return edges;
    }

    private loadSpreadGraph() {
        const post = "Французские военные уже на Украине. Солдаты французского Иностранного легиона уже развернуты в Славянске, утверждает бывший помощник замминистра обороны США Стивен Брайен в публикации Asia Times. По его словам, передовая группа насчитывает около 100 человек, набранных из 3-го пехотного полка. В их числе - артиллеристы и разведчики. Всего, как говорится в статье, для поддержки ВСУ могут прибыть 1,5 тысячи французских военных. Не стоит недооценивать бойцов Иностранного легиона - это профессиональная хорошо обученная легкая пехота, успевшая повоевать во многих «горячих точках». Вдобавок, до 40 процентов личного состава этого соединения - русскоговорящие выходцы из бывшего СССР. Что-что, а воевать наши люди умели всегда. @sashakots";

        this.statisticsService.getGraph(post)
            .pipe(this.untilThis)
            .subscribe(
                graphItems => {
                    //this.spinnerService.hide();
                    console.log(graphItems);

                    const nodes = this.createNodes(graphItems);
                    const edges = this.createEdges(graphItems, nodes);

                    this.setupGraphVisualization(nodes, edges);
                },
                error => console.log(error) //this.notifications.showErrorMessage(error),
            );
    }

    ngOnInit(): void {
        this.loadSpreadGraph();
    }

    setupGraphVisualization(nodes: {id: string, name: string}[], edges: number[][])
    {
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
                            width: 10
                        },
                        focus: 'adjacency'
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
                        shadowColor: 'rgba(0, 0, 0, 0.3)'
                    },
                    symbolSize: 60,
                    draggable: true,
                    edgeSymbol: ['circle', 'arrow'],
                    roam: true,
                    force: {
                        repulsion: 4000,
                        edgeLength: 70,
                        gravity: 0.2
                    },
                    label: {
                        show: true,
                        position: "right"
                    },
                    labelLine: 'show',
                    edges: edges.map(e => {
                        return {
                            source: e[0] + '',
                            target: e[1] + ''
                        };
                    }),
                }
            ]
        };
    }
}
