function [] = plotHomographyResult(units,image,checker,coord,M,N)

figure
if ~isempty(image)
    subplot(1,3,1)
    imshow(image,[min(image(:)) max(image(:))]);
    hold on
    plot(checker(:,1),checker(:,2),'or');
end

est = zeros(size(checker',1),size(checker',2),2);
act = zeros(size(checker',1),size(checker',2),2);
error = zeros(size(checker',1),size(checker',2),2);
act(:,:,1) = coord';
act(:,:,2) = checker';
est(1:2,:,1) = homography_transform(act(:,:,2),M);
est(1:2,:,2) = homography_transform(act(:,:,1),N);
error(:,:,1) = act(:,:,1) - est(:,:,1);
error(:,:,2) = act(:,:,2) - est(:,:,2);

for i = 1:2
    subplot(1,3,i+1);hold on;
    title(strcat('X:',num2str(round(mean(abs(error(1,:,i))),3)),'±',num2str(round(std(abs(error(1,:,i))),3)),units{3-i},' _ Y:',num2str(round(mean(abs(error(2,:,i))),3)),'±',num2str(round(std(abs(error(2,:,i))),3)),units{3-i}));
    xlabel(strcat('[Max Error] X:',num2str(round(max(abs(error(1,:,i))),3)),units{3-i},'_ Y:',num2str(round(max(abs(error(2,:,i))),3)),units{3-i}));
    plot(est(1,:,i),est(2,:,i),'ro',act(1,:,i),act(2,:,i),'bo');
    quiver(act(1,:,i),act(2,:,i),error(1,:,i),error(2,:,i))
    plot(est(1,1:2,i),est(2,1:2,i),'r*',act(1,1:2,i),act(2,1:2,i),'b*');
end

end

