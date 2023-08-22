#include <stdio.h>
#include <string.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <sys/types.h>
#include <netinet/in.h>
#include <stdlib.h>
#include <pthread.h>

	
#define PORT	8080
#define MAXLINE 1600
int serverSock() {
    int sockfd;
	struct sockaddr_in servaddr;
	// Creating socket file descriptor
	if ( (sockfd = socket(AF_INET, SOCK_DGRAM, 0)) < 0 ) {
		perror("socket creation failed");
		exit(1);
	}
	memset(&servaddr, 0, sizeof(servaddr));
	// Filling server information
	servaddr.sin_family = AF_INET; // IPv4
	servaddr.sin_addr.s_addr = INADDR_ANY;
	servaddr.sin_port = htons(8081);
	// Bind the socket with the server address
	if ( bind(sockfd, (const struct sockaddr *)&servaddr,
			sizeof(servaddr)) < 0 )
	{
		perror("bind failed");
		exit(0);
	}
    return sockfd;
}
struct sockaddr_in server_addr;
struct sockaddr cliaddr;
int sock;

int server() {
    
	char buffer[MAXLINE];
	int len, n;
	
	len = sizeof(cliaddr); //len is value/result
    while(1) {
        bzero(&cliaddr, len);
	    n = recvfrom(sock, (char *)buffer, MAXLINE,
				MSG_WAITALL, ( struct sockaddr *) &cliaddr,
				&len);
	    buffer[n] = '\0';
        printf("received from client: %d\n", buffer[0]);
	    sendto(sock, (const char *)buffer, 1,
		    0, (const struct sockaddr *) &server_addr,
			    len);
        printf("sent to server: %s\n", buffer);
    }	
	return 0;
}
int client() {
    int server_struct_length = sizeof(server_addr);
    printf("Socket created successfully\n");
    // Set port and IP:
    // Get input from the user:
    // Send the message to server:
    char buff[MAXLINE];
    while (1) {
        bzero(buff, MAXLINE);
    // Receive the server's response:
        int n;
        if(n = recvfrom(sock, buff, MAXLINE, 0,
            (struct sockaddr*)&server_addr, &server_struct_length) < 0){
            printf("Error while receiving server's msg\n");
            return -1;
        }
        buff[n] = 0;
        printf("received from server: %s", buff);
        if(sendto(sock, buff, n, 0,
            (struct sockaddr*)&cliaddr, server_struct_length) < 0){
            printf("Unable to send message\n");
            return -1;
        }
    }
    // Close the socket:
    return 0;
}
int main(void){
    
    server_addr.sin_family = AF_INET;
    server_addr.sin_port = htons(PORT);
    server_addr.sin_addr.s_addr = inet_addr("127.0.0.1");
    sock = serverSock();
    // serversock = serverSock();
    // pthread_t thread_id;
    // // pthread_create(&thread_id, NULL, server(), NULL);
    // // client();
    // // pthread_join(thread_id, NULL);
    // close(clientsock);
    // close(serversock);
    // return 0;
    char server_message[2000], client_message[2000];
    int server_struct_length = sizeof(server_addr);
    
    // Clean buffers:
    memset(server_message, '\0', sizeof(server_message));
    memset(client_message, '\0', sizeof(client_message));
    
    
    // Get input from the user:
    // Send the message to server:
    while (1) {
        printf("Enter message: ");
        gets(client_message);
        printf("you have entered %s\n", client_message);
        if(sendto(sock, client_message, strlen(client_message), 0,
            (struct sockaddr*)&server_addr, server_struct_length) < 0){
            printf("Unable to send message\n");
            return -1;
        }
    
    // Receive the server's response:
        // if(recvfrom(sock, server_message, sizeof(server_message), 0,
        //     (struct sockaddr*)&server_addr, &server_struct_length) < 0){
        //     printf("Error while receiving server's msg\n");
        //     return -1;
        // }
        // printf("Server's response: %d\n", server_message[0]);
    }
    // Close the socket:
    close(sock);
    
    return 0;
}


