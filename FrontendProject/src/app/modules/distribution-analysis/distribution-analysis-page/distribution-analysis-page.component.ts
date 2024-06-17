import { formatDate } from '@angular/common';
import { Component, Inject, LOCALE_ID, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BaseComponent } from '@core/base/base.component';
import { DistributionGraphNode } from '@core/models/distribution-graph-node';
import { NotificationService } from '@core/services/notification.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { EChartsOption } from 'echarts';
import { DistributionAnalysisService } from "@core/services/distribution-analysis.service";
import { ActivatedRoute } from "@angular/router";
import * as echarts from 'echarts';

@Component({
    selector: 'app-distribution-analysis-page',
    templateUrl: './distribution-analysis-page.component.html',
    styleUrls: ['./distribution-analysis-page.component.sass'],
})
export class DistributionAnalysisPageComponent extends BaseComponent implements OnInit {
    // @ts-ignore
    graphOptions: EChartsOption;

    // @ts-ignore
    treeOptions: EChartsOption;

    // @ts-ignore
    calendarOptions: EChartsOption;

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
        private activateRoute: ActivatedRoute
    ) {
        super();
    }

    ngOnInit() {
        this.activateRoute.params.pipe(this.untilThis).subscribe((params) => {
            const post = params['post-text'];
            if (post)
            {
                this.requestLoadDistributionGraph(post);
            }
        });
    }

    loadDistributionGraph() {
        const post = this.searchForm.value.postText!;

        this.requestLoadDistributionGraph(post);
    }

    requestLoadDistributionGraph(post: string) {
        this.spinnerService.show();
        this.analysisService.getPostDistributionGraph(post)
            .pipe(this.untilThis)
            .subscribe(
                graphNodes => {
                    this.spinnerService.hide();
                    console.log(graphNodes);

                    const nodes = this.createNodes(graphNodes);
                    const edges = this.createEdges(graphNodes, nodes);

                    this.setupGraphVisualization(nodes, edges);

                    const tree = this.createTreeStructure(graphNodes);
                    this.setupTreeVisualization(tree);

                    this.setupCalendarVisualization(graphNodes)
                },
                error => {
                    this.spinnerService.hide();

                    console.log(error);
                    this.notifications.showWarningMessage(`Сталася помилка!
                    Ваше повідомлення не знайдено серед повідомлень, які були отримані в результаті парсингу`);
                },
            );
    }

    createEdges(graphNodes: DistributionGraphNode[], nodes: { id: string, name: string }[]) {
        const edges = [];

        for (let i = 0; i < nodes.length; i++) {
            const nodeItem = graphNodes[i];
            const node = nodes[i];

            const parentNodeId = nodeItem.root_id;

            if (parentNodeId != null) {
                const parentNodeIndex = graphNodes.findIndex(n => n.post_id === parentNodeId);

                if (parentNodeIndex >= 0) {
                    edges.push([+nodes[parentNodeIndex].id, +node.id]);
                }
            }
        }

        return edges;
    }

    createNodes(nodeItems: DistributionGraphNode[]) {
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

    createTreeStructure(nodeItems: DistributionGraphNode[])  {
        const nodeMap = new Map();
        const rootNodes: DistributionTreeNode[] = [];
        const nodes: DistributionTreeNode[] = [];

        nodeItems.forEach(node => {
            const parent = nodeItems.find(n => n.post_id === node.root_id);

            if (parent && parent.channel_title === node.channel_title)
                return;

            const treeNode = {
                children: [],
                name: node.channel_title,
                value: Math.floor(Math.random() * 10000),
                parent: parent?.channel_title,
                post: node
            };

            const existingNode = nodeMap.get(node.channel_title);

            if (existingNode && existingNode.post.created_at < treeNode.post.created_at) {
                return;
            }

            nodes.push(treeNode);
            nodeMap.set(treeNode.name, treeNode);
        });

        nodes.forEach(node => {
            if (node.parent !== undefined && nodeMap.has(node.parent)) {
                nodeMap.get(node.parent).children.push(node);
            } else {
                rootNodes.push(node);
            }
        });

        console.log(rootNodes);

        return rootNodes[0];
    }

    setupGraphVisualization(nodes: { id: string, name: string }[], edges: number[][]) {
        this.graphOptions = {
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

    setupTreeVisualization(tree: DistributionTreeNode) {
        this.treeOptions = {
            series: [
                {
                    type: 'tree',
                    data: [tree],
                    symbolSize: 20,
                    label: {
                        position: 'left',
                        verticalAlign: 'middle',
                        align: 'right',
                        fontSize: 20,
                    },
                    leaves: {
                        label: {
                            position: 'right',
                            verticalAlign: 'middle',
                            align: 'left',
                        },
                    },
                    expandAndCollapse: true,
                    animationDuration: 550,
                    animationDurationUpdate: 750,
                },
            ],
        };
    }

    getVirtualData(year: string) {
        const date = +echarts.time.parse(year + '-01-01');
        const end = +echarts.time.parse(+year + 1 + '-01-01');
        const dayTime = 3600 * 24 * 1000;
        const data: [string, number][] = [];
        for (let time = date; time < end; time += dayTime) {
            data.push([
                echarts.time.format(time, '{yyyy}-{MM}-{dd}', false),
                Math.floor(Math.random() * 1000)
            ]);
        }
        return data;
    }

    formatDate(date: Date) {
        return `${date.getFullYear()}-${date.getMonth().toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
    }

    setupCalendarVisualization(nodes: DistributionGraphNode[]) {
        const oldestDate = nodes.reduce((oldest, node) => {
            return new Date(node.created_at) < oldest ? new Date(node.created_at) : oldest;
        }, new Date(nodes[0].created_at));

        const newestDate = nodes.reduce((newest, node) => {
            return new Date(node.created_at) > newest ? new Date(node.created_at) : newest;
        }, new Date(nodes[0].created_at));

        const oldestDateStr = this.formatDate(oldestDate);
        const newestDateStr = this.formatDate(newestDate);


        const graphData: [string, number][] = nodes.map(node => [this.formatDate(new Date(node.created_at)), 200]);

        const links = graphData.map(function (item, idx) {
            return {
                source: idx,
                target: idx + 1
            };
        });
        links.pop();

        this.calendarOptions = {
            tooltip: {},
            calendar: {
                top: 'middle',
                left: 'center',
                orient: 'vertical',
                cellSize: 40,
                yearLabel: {
                    margin: 50,
                    fontSize: 30
                },
                dayLabel: {
                    firstDay: 1,
                    nameMap: 'cn'
                },
                monthLabel: {
                    nameMap: 'cn',
                    margin: 15,
                    fontSize: 20,
                    color: '#999'
                },
                range: [oldestDateStr, newestDateStr]
            },
            visualMap: {
                min: 0,
                max: 1000,
                type: 'piecewise',
                left: 'center',
                bottom: 20,
                inRange: {
                    color: ['#5291FF', '#C7DBFF']
                },
                seriesIndex: [1],
                orient: 'horizontal'
            },
            series: [
                {
                    type: 'graph',
                    edgeSymbol: ['none', 'arrow'],
                    coordinateSystem: 'calendar',
                    links: links,
                    symbolSize: 15,
                    calendarIndex: 0,
                    itemStyle: {
                        color: 'yellow',
                        shadowBlur: 9,
                        shadowOffsetX: 1.5,
                        shadowOffsetY: 3,
                        shadowColor: '#555'
                    },
                    lineStyle: {
                        color: '#D10E00',
                        width: 1,
                        opacity: 1
                    },
                    data: graphData,
                    z: 20
                },
                {
                    type: 'heatmap',
                    coordinateSystem: 'calendar',
                    data: this.getVirtualData(oldestDate.getFullYear().toString())
                }
            ]
        };
    }
}

interface DistributionTreeNode
{
    name: string;
    value: number;
    children: DistributionTreeNode[];
    parent: string | undefined;
    post: DistributionGraphNode;
}
